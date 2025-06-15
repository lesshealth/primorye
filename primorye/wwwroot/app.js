const api = "https://localhost:7003/api";
let currentTeamId = null;
let currentCityId = null;

async function login() {
    const res = await fetch(`${api}/auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            login: document.getElementById("login").value,
            password: document.getElementById("password").value
        })
    });
    const data = await res.json();
    if (res.ok) {
        document.getElementById("loginResult").textContent = `Вы вошли: ${data.login}`;
    } else {
        document.getElementById("loginResult").textContent = `Ошибка: ${data.message}`;
    }
}

async function loadCities() {
    const res = await fetch(`${api}/city`);
    const data = await res.json();
    const select = document.getElementById("citySelect");
    select.innerHTML = "";
    data.forEach(city => {
        const option = document.createElement("option");
        option.value = city.id;
        option.textContent = city.name;
        select.appendChild(option);
    });

    document.getElementById("startButton").disabled = true;
}

async function selectCity() {
    currentCityId = parseInt(document.getElementById("citySelect").value);
    if (!isNaN(currentCityId)) {
        document.getElementById("startButton").disabled = false;
        alert("Вы выбрали город. Теперь нажмите Старт игры.");
    }
}

async function startGame() {
    currentTeamId = parseInt(document.getElementById("teamId").value);
    const res = await fetch(`${api}/game/start?teamId=${currentTeamId}&cityId=${currentCityId}`, { method: "POST" });
    const data = await res.json();
    renderState(data);
    alert("Игра началась!");
    await loadNextQuestion();
}

async function loadNextQuestion() {
    const res = await fetch(`${api}/game/next-question?teamId=${currentTeamId}`);
    if (!res.ok) {
        const data = await res.json();
        alert(data.message || "Ошибка при загрузке вопроса");
        return;
    }
    const question = await res.json();
    renderQuestion(question);
}

function renderQuestion(data) {
    document.getElementById("questionText").textContent = data.text;
    const container = document.getElementById("variants");
    container.innerHTML = "";
    data.variants.forEach(v => {
        const btn = document.createElement("button");
        btn.textContent = v.text;
        btn.onclick = () => submitAnswer(data.id, v.id);
        container.appendChild(btn);
    });
}

async function submitAnswer(questionId, variantId) {
    const res = await fetch(`${api}/game/answer-from-db`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ teamId: currentTeamId, questionId, variantId })
    });

    const data = await res.json();
    renderState(data);

    if (data.stepNumber === 1 && data.waitingForAnswer) {
        await loadNextQuestion();
    }
}

async function choose(action, cost, effectOnPoints, effectOnProgress) {
    const res = await fetch(`${api}/game/choose`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ teamId: currentTeamId, action, cost, effectOnPoints, effectOnProgress })
    });
    const data = await res.json();
    renderState(data);
}

function renderState(state) {
    document.getElementById("finance").textContent = state.finance;
    document.getElementById("socialPoints").textContent = state.socialPoints;
    document.getElementById("progress").textContent = state.progress;
    document.getElementById("goal").textContent = state.currentGoal;
}

window.onload = () => {
    loadCities();
};
