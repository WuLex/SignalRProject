"use strict";

// 建立SignalR连接
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/messages", {
        accessTokenFactory: () => "testing" // 使用测试访问令牌
    })
    .build();

// 接收消息的处理函数
connection.on("ReceiveMessage", function (message) {
    var msg = message
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;"); // 防止HTML注入
    var div = document.createElement("div");
    div.innerHTML = msg + "<hr/>";
    document.getElementById("messages").appendChild(div);
});

// 用户连接的处理函数
connection.on("UserConnected", function (connectionId) {
    var groupElement = document.getElementById("group");
    var option = document.createElement("option");
    option.text = connectionId;
    option.value = connectionId;
    groupElement.add(option);

    // 重新渲染layui的select组件
    layui.form.render('select');
});

// 用户断开的处理函数
connection.on("UserDisconnected", function (connectionId) {
    var groupElement = document.getElementById("group");
    for (var i = 0; i < groupElement.length; i++) {
        if (groupElement.options[i].value === connectionId) {
            groupElement.remove(i);
            break; // 找到后立即退出循环
        }
    }

    // 重新渲染layui的select组件
    layui.form.render('select');
});

// 开始连接
connection.start().catch(function (err) {
    console.error(err.toString());
});

// 发送消息按钮的点击事件处理函数
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

    event.preventDefault(); // 阻止默认的表单提交行为
});

// 加入组按钮的点击事件处理函数
document.getElementById("joinGroup").addEventListener("click", function (event) {
    connection.invoke("JoinGroup", "PrivateGroup").catch(function (err) {
        console.error(err.toString());
    });
    event.preventDefault(); // 阻止默认的表单提交行为
});
