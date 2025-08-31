const apiUrl = "http://localhost:5227/api";
let token = "";

async function login() {
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    try {
        const res = await fetch(`${apiUrl}/Auth/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, passwordHash: password })
        });

        if (!res.ok) throw new Error("Fel användarnamn eller lösenord");

        const data = await res.json();
        token = data.token;

        document.getElementById("loginDiv").style.display = "none";
        document.getElementById("todoDiv").style.display = "block";

        await getTodos();
    } catch (err) {
        console.error(err);
        alert("Ett fel uppstod vid inloggning.");
    }
}

async function getTodos() {
    const res = await fetch(`${apiUrl}/Todos`, {
        headers: { "Authorization": `Bearer ${token}` }
    });
    if (!res.ok) { alert("Kunde inte hämta todos."); return; }

    const todos = await res.json();
    const list = document.getElementById("todoList");
    list.innerHTML = "";

    todos.forEach(todo => {
        const li = document.createElement("li");
        li.textContent = todo.title + " ";
        if (todo.isDone) li.classList.add("done");

        const doneBtn = document.createElement("button");
        doneBtn.textContent = todo.isDone ? "❌" : "✔️";
        doneBtn.onclick = () => updateTodo(todo.id, todo.title, !todo.isDone);

        const delBtn = document.createElement("button");
        delBtn.textContent = "Ta bort";
        delBtn.onclick = () => deleteTodo(todo.id);

        li.appendChild(doneBtn);
        li.appendChild(delBtn);
        list.appendChild(li);
    });
}

async function addTodo() {
    const title = document.getElementById("newTodo").value;
    if (!title) return;

    await fetch(`${apiUrl}/Todos`, {
        method: "POST",
        headers: { "Content-Type": "application/json", "Authorization": `Bearer ${token}` },
        body: JSON.stringify({ title, isDone: false })
    });

    document.getElementById("newTodo").value = "";
    await getTodos();
}

async function updateTodo(id, title, isDone) {
    await fetch(`${apiUrl}/Todos/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json", "Authorization": `Bearer ${token}` },
        body: JSON.stringify({ title, isDone })
    });

    await getTodos();
}

async function deleteTodo(id) {
    await fetch(`${apiUrl}/Todos/${id}`, {
        method: "DELETE",
        headers: { "Authorization": `Bearer ${token}` }
    });

    await getTodos();
}

function logout() {
    token = "";
    document.getElementById("loginDiv").style.display = "block";
    document.getElementById("todoDiv").style.display = "none";
}
