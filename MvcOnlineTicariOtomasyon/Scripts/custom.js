function sendMessageToController(message) {
    $.ajax({
        type: "POST",
        url: "/Log/SendMessageToConsole",
        data: { message: message },
        success: function (response) {
            console.log("Mesaj başarıyla kontrolöre iletilmiştir.");
        },
        error: function (xhr, status, error) {
            console.error("Mesaj kontrolöre iletilirken bir hata oluştu.");
        }
    });
}
