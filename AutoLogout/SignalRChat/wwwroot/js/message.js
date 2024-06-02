"use strict";

// ����SignalR����
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/messages", {
        accessTokenFactory: () => "testing" // ʹ�ò��Է�������
    })
    .build();

// ������Ϣ�Ĵ�����
connection.on("ReceiveMessage", function (message) {
    var msg = message
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;"); // ��ֹHTMLע��
    var div = document.createElement("div");
    div.innerHTML = msg + "<hr/>";
    document.getElementById("messages").appendChild(div);
});

// �û����ӵĴ�����
connection.on("UserConnected", function (connectionId) {
    var groupElement = document.getElementById("group");
    var option = document.createElement("option");
    option.text = connectionId;
    option.value = connectionId;
    groupElement.add(option);

    // ������Ⱦlayui��select���
    layui.form.render('select');
});

// �û��Ͽ��Ĵ�����
connection.on("UserDisconnected", function (connectionId) {
    var groupElement = document.getElementById("group");
    for (var i = 0; i < groupElement.length; i++) {
        if (groupElement.options[i].value === connectionId) {
            groupElement.remove(i);
            break; // �ҵ��������˳�ѭ��
        }
    }

    // ������Ⱦlayui��select���
    layui.form.render('select');
});

// ��ʼ����
connection.start().catch(function (err) {
    console.error(err.toString());
});

// ������Ϣ��ť�ĵ���¼�������
document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("message").value;
    var groupElement = document.getElementById("group");
    var groupValue = groupElement.options[groupElement.selectedIndex].value;

    if (groupValue === "All" || groupValue === "Myself") {
        var method = (groupValue === "All") ? "SendMessageToAll" : "SendMessageToCaller";
        connection.invoke(method, message).catch(function (err) {
            console.error(err.toString());
        });
    } else if (groupValue === "PrivateGroup") {
        connection.invoke("SendMessageToGroup", "PrivateGroup", message).catch(function (err) {
            console.error(err.toString());
        });
    } else {
        connection.invoke("SendMessageToUser", groupValue, message).catch(function (err) {
            console.error(err.toString());
        });
    }

    event.preventDefault(); // ��ֹĬ�ϵı��ύ��Ϊ
});

// �����鰴ť�ĵ���¼�������
document.getElementById("joinGroup").addEventListener("click", function (event) {
    connection.invoke("JoinGroup", "PrivateGroup").catch(function (err) {
        console.error(err.toString());
    });
    event.preventDefault(); // ��ֹĬ�ϵı��ύ��Ϊ
});
