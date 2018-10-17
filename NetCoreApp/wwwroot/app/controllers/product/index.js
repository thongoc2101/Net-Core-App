var productController = function() {
    this.initialize = function() {
        loadData();
    }

    function registerEvents() {

    }

    function loadData() {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: "/admin/product/GetAll",
            dataType: 'json',
            success: function(response) {
                $.each(response,
                    function(i, item) {
                        render += Mustache.render(template,
                            {
                                Id: item.Id,
                                Name: item.Name,
                                Price: item.Price,
                                Category: item.ProductCategory.Name,
                                Image: item.image,
                                CreatedDate: item.DateCreated,
                                Status: item.Status
                            });
                        if (render !== '') {
                            $('#table-content').html(render);
                        }
                    });
            },
            error: function(status) {
                console.log(status);
                app.notify('Cannot loading data ', 'error');
            }
        });
    }
}