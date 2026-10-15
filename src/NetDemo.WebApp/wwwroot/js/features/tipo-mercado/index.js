const page = document.querySelector("#tipoMercadoPage");

if (page) {
    const endpoint = page.dataset.endpoint;
    const tableBody = document.querySelector("#tipoMercadoTable tbody");
    const emptyRow = document.querySelector("#tipoMercadoEmptyRow");
    const message = document.querySelector("#tipoMercadoMessage");
    const registerForm = document.querySelector("#registrarForm");
    const editForm = document.querySelector("#editarForm");
    const editModal = document.querySelector("#editarModal");

    function showMessage(text, isError = false) {
        message.textContent = text;
        message.className = `alert ${isError ? "alert-danger" : "alert-success"}`;
    }

    function clearMessage() {
        message.textContent = "";
        message.className = "alert d-none";
    }

    function clearFormError(form) {
        const error = form.querySelector("[data-form-error]");
        error.textContent = "";
        error.classList.add("d-none");
    }

    function showFormError(form, text) {
        const error = form.querySelector("[data-form-error]");
        error.textContent = text;
        error.classList.remove("d-none");
    }

    function getErrorMessage(payload) {
        if (payload?.errors) {
            const errors = Object.values(payload.errors).flat();
            if (errors.length > 0) {
                return errors.join(" ");
            }
        }

        return payload?.detail ?? payload?.title ?? "No se pudo completar la operación.";
    }

    async function sendRequest(url, method, form = null) {
        const token = form?.querySelector('input[name="__RequestVerificationToken"]')
            ?.value ?? document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        const options = {
            method,
            headers: { RequestVerificationToken: token },
            credentials: "same-origin"
        };

        if (form) {
            options.body = new FormData(form);
        }

        const response = await fetch(url, options);
        if (response.status === 204) {
            return null;
        }

        const payload = await response.json().catch(() => null);
        if (!response.ok) {
            throw new Error(getErrorMessage(payload));
        }

        return payload;
    }

    function updateEmptyState() {
        const hasItems = tableBody.querySelector("tr[data-id]") !== null;
        emptyRow.classList.toggle("d-none", hasItems);
    }

    function createCell(className, text) {
        const cell = document.createElement("td");
        if (className) {
            cell.className = className;
        }
        cell.textContent = text;
        return cell;
    }

    function createRow(tipo) {
        const row = document.createElement("tr");
        row.dataset.id = tipo.id;
        row.dataset.nombre = tipo.nombre;
        row.dataset.estado = tipo.estado;

        row.append(createCell("tipo-mercado-id", tipo.id));
        row.append(createCell("tipo-mercado-nombre", tipo.nombre));

        const stateCell = document.createElement("td");
        const stateBadge = document.createElement("span");
        stateBadge.className = `badge ${tipo.estado === "Activo" ? "text-bg-success" : "text-bg-secondary"} tipo-mercado-estado`;
        stateBadge.textContent = tipo.estado;
        stateCell.append(stateBadge);
        row.append(stateCell);

        const actionsCell = document.createElement("td");
        actionsCell.className = "text-end text-nowrap";

        const editButton = document.createElement("button");
        editButton.className = "btn btn-sm btn-outline-primary";
        editButton.type = "button";
        editButton.dataset.action = "editar";
        editButton.dataset.bsToggle = "modal";
        editButton.dataset.bsTarget = "#editarModal";
        editButton.textContent = "Editar";

        const deleteButton = document.createElement("button");
        deleteButton.className = "btn btn-sm btn-outline-danger ms-2";
        deleteButton.type = "button";
        deleteButton.dataset.action = "eliminar";
        deleteButton.textContent = "Eliminar";

        actionsCell.append(editButton, deleteButton);
        row.append(actionsCell);
        return row;
    }

    function applyTipoToRow(row, tipo) {
        row.dataset.id = tipo.id;
        row.dataset.nombre = tipo.nombre;
        row.dataset.estado = tipo.estado;
        row.querySelector(".tipo-mercado-id").textContent = tipo.id;
        row.querySelector(".tipo-mercado-nombre").textContent = tipo.nombre;

        const stateBadge = row.querySelector(".tipo-mercado-estado");
        stateBadge.textContent = tipo.estado;
        stateBadge.className = `badge ${tipo.estado === "Activo" ? "text-bg-success" : "text-bg-secondary"} tipo-mercado-estado`;
    }

    registerForm.addEventListener("submit", async (event) => {
        event.preventDefault();
        clearMessage();

        if (!registerForm.reportValidity()) {
            return;
        }

        clearFormError(registerForm);

        try {
            const tipo = await sendRequest(endpoint, "POST", registerForm);
            tableBody.insertBefore(createRow(tipo), emptyRow);
            updateEmptyState();
            bootstrap.Modal.getOrCreateInstance(document.querySelector("#registrarModal")).hide();
            registerForm.reset();
            showMessage("Tipo de mercado registrado.");
        } catch (error) {
            showFormError(registerForm, error.message);
        }
    });

    editModal.addEventListener("show.bs.modal", (event) => {
        const row = event.relatedTarget?.closest("tr[data-id]");
        if (!row) {
            return;
        }

        clearMessage();
        clearFormError(editForm);
        editForm.elements.Id.value = row.dataset.id;
        editForm.elements.Nombre.value = row.dataset.nombre;
        editForm.elements.Estado.value = row.dataset.estado;
    });

    editForm.addEventListener("submit", async (event) => {
        event.preventDefault();
        clearMessage();

        if (!editForm.reportValidity()) {
            return;
        }

        clearFormError(editForm);

        const id = editForm.elements.Id.value;
        try {
            const tipo = await sendRequest(`${endpoint}/${encodeURIComponent(id)}`, "PUT", editForm);
            const row = [...tableBody.querySelectorAll("tr[data-id]")]
                .find((item) => item.dataset.id === tipo.id);

            if (row) {
                applyTipoToRow(row, tipo);
            }

            bootstrap.Modal.getOrCreateInstance(editModal).hide();
            showMessage("Tipo de mercado actualizado.");
        } catch (error) {
            showFormError(editForm, error.message);
        }
    });

    tableBody.addEventListener("click", async (event) => {
        const button = event.target.closest("button[data-action='eliminar']");
        if (!button) {
            return;
        }

        const row = button.closest("tr[data-id]");
        if (!row || !window.confirm(`¿Eliminar el tipo de mercado "${row.dataset.nombre}"? Se borrará su archivo local.`)) {
            return;
        }

        clearMessage();
        button.disabled = true;

        try {
            await sendRequest(`${endpoint}/${encodeURIComponent(row.dataset.id)}`, "DELETE");
            row.remove();
            updateEmptyState();
            showMessage("Tipo de mercado eliminado.");
        } catch (error) {
            button.disabled = false;
            showMessage(error.message, true);
        }
    });

    document.querySelector("#registrarModal").addEventListener("hidden.bs.modal", () => {
        registerForm.reset();
        clearFormError(registerForm);
    });

    editModal.addEventListener("hidden.bs.modal", () => {
        editForm.reset();
        clearFormError(editForm);
    });
}
