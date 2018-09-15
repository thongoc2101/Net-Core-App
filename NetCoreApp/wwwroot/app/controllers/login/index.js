var loginController = function() {
    this.initialize = function() {
        registerEvents();
    }

    var registerEvents = function() {
        $('#btnLogin').on('click',
            function(e) {
                e.preventDefault();
                var user = $('#txtUseName').val();
                var password = $('#txtPassword').val();
                login(user, password);
            });
    }

    var login = function(user, pass) {
        $.ajax({
            type: "POST",
            data: {
                UserName: user,
                Password: pass
            },
            dataType: "json",
            url: "/admin/login/authen",
            success: function(res) {
                if (res.Success) {
                    app.notify("Login success", "success");
                    window.location.href = "/Admin/Home/Index";
                } else {
                    app.notify("Đăng nhập không đúng", "error");
                }
            }
        });
    }

}
