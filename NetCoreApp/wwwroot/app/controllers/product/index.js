var productController = function() {
    this.initialize = function() {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $('#ddlShowPage').on('change', function () {
            app.configs.pageSize = $(this).val();
            app.configs.pageIndex = 1;
            loadData(true);
        });
    }

    function loadData(isPageChanged) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: "/admin/product/GetAllPaging",
            data: {
                categoryId: null,
                keyword: $('#txtKeyword').val(),
                page: app.configs.pageIndex,
                pageSize: app.configs.pageSize
            },
            dataType: 'json',
            success: function(response) {
                if (response.length === 0) {
                    app.notify('No data', 'error');
                } else {
                    $.each(response.Results,
                        function(i, item) {
                            render += Mustache.render(template,
                                {
                                    Id: item.Id,
                                    Name: item.Name,
                                    Price: item.Price,
                                    Category: item.ProductCategory.Name,
                                    Image: item.image= '<img src="/vendor/gentelella/production/user.png" width = "25px"></img>',
                                    CreatedDate: app.dateFormatJson(item.DateCreated),
                                    Status: app.getStatus(item.Status)
                                });
                            if (render !== '') {
                                $('#lblTotalRecords').text(response.RowCount);
                                $('#table-content').html(render);
                            }
                            wrapPaging(response.RowCount, function () {
                                loadData();
                            }, isPageChanged);
                        });
                }
            },
            error: function(status) {
                console.log(status);
                app.notify('Cannot loading data ', 'error');
            }
        });
    }

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalSize = Math.ceil(recordCount / app.configs.pageSize);
        //Unbind pagination if it existed or click change pageSize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalSize,
            visiblePages: 7,
            first: 'First',
            prev: 'Previous',
            next: 'Next',
            last: 'Last',
            onPageClick: function (event, p) {
                //Assign if p!==CurrentPage
                if (app.configs.pageIndex !== p) {
                    app.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            }
        });
    }
}