var loginController = function() {
    this.initialize = function() {
        registerEvents();
    }

    var registerEvents = function() {
        $('#frmLogin').validate({
            errorClass: 'red',
            ignore: [],
            rule: {
                userName: {
                    required: true
                },
                password: {
                    required: true
                }
            }
        });

        $('#btnLogin').on('click',
            function(e) {
                if ($('#frmLogin').valid()) {
                    e.preventDefault();
                    var user = $('#txtUserName').val();
                    var password = $('#txtPassword').val();
                    login(user, password);
                }
            });
    }

var login = function(user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                UserName: user,
                Password: pass
            },
            dataType: "json",
            url: "/admin/login/Authen",
            success: function(res) {
                if (res.Success) {
                    app.notify("Login success", "success");
                    window.location.href = "/Admin/Home/Index";
                } else {
                    app.notify("Login Failed", "error");
                }
            }
        });
    }

}
