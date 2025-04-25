// wwwroot/js/chat.js
function initializeChat(username) {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`/chatHub?username=${username}`)
        .configureLogging(signalR.LogLevel.Information)
        .build();

    // Message handling
    connection.on("ReceiveMessage", (message) => {
        const isCurrentUser = message.sender === username;
        const messageDiv = document.createElement("div");
        messageDiv.className = `message ${isCurrentUser ? "user-message" : "other-message"}`;

        messageDiv.innerHTML = `
            <div>
                ${!isCurrentUser ? `<span class="message-username">${message.sender}</span>` : ''}
                ${message.content}
            </div>
            <span class="message-time">${new Date(message.sentAt).toLocaleTimeString()}</span>
        `;

        document.querySelector(".chat-messages").appendChild(messageDiv);
        document.querySelector(".chat-messages").scrollTop =
            document.querySelector(".chat-messages").scrollHeight;
    });

    // User list updates
    connection.on("UserConnected", (username) => {
        updateUserList();
    });

    connection.on("UserDisconnected", (username) => {
        updateUserList();
    });

    // Start connection
    connection.start()
        .then(() => {
            document.getElementById("send-btn").addEventListener("click", sendMessage);
            document.getElementById("message-input").addEventListener("keypress", (e) => {
                if (e.key === "Enter") sendMessage();
            });

            document.getElementById("leave-btn").addEventListener("click", () => {
                connection.invoke("SendMessage", "!leave")
                    .then(() => window.location.href = "/");
            });
        })
        .catch(console.error);

    function sendMessage() {
        const input = document.getElementById("message-input");
        const message = input.value.trim();
        if (message) {
            connection.invoke("SendMessage", message)
                .catch(console.error);
            input.value = "";
        }
    }

    async function updateUserList() {
        const response = await fetch("/Chat/GetActiveUsers");
        const users = await response.json();

        document.querySelector(".user-count").textContent =
            `${users.length} ${users.length === 1 ? 'user' : 'users'} online`;

        const container = document.querySelector(".users-container");
        container.innerHTML = users.map(user => `
            <div class="user-item">
                <div class="user-avatar" style="background-color: ${getUserColor(user.username)}">
                    ${user.username[0]}
                </div>
                <div class="user-info">
                    <div class="username">${user.username}</div>
                    <div class="connect-time">since ${new Date(user.connectedAt).toLocaleTimeString()}</div>
                </div>
            </div>
        `).join("");
    }

    function getUserColor(username) {
        let hash = 0;
        for (let i = 0; i < username.length; i++) {
            hash = username.charCodeAt(i) + ((hash << 5) - hash);
        }
        const hue = Math.abs(hash) % 360;
        return `hsl(${hue}, 70%, 60%)`;
    }
}