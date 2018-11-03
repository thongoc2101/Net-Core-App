var RoleController = function() {
    var self = this;
    this.initialize = function() {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        // Init validate
        $('#frmMaintenance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtName: { required: true }
            }
        });

        $('#txt-search-keyword').keypress(function(e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });

        $("#btn-search").on('click',
            function() {
                loadData();
            });

        $("#ddl-show-page").on('change',
            function() {
                app.configs.pageSize = $(this).val();
                app.configs.pageIndex = 1;
                loadData(true);
            });

        $("#btn-create").on('click',
            function() {
                resetFormMaintenance();
                $('#modal-add-edit').modal('show');

            });

        $('body').on('click',
            '.btn-edit',
            function(e) {
                e.preventDefault();
                var that = $(this).data('id');
                $.ajax({
                    type: "GET",
                    url: "/Admin/Role/GetById",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function() {
                        app.startLoading();
                    },
                    success: function(response) {
                        var data = response;
                        $('#hidId').val(data.Id);
                        $('#txtName').val(data.Name);
                        $('#txtDescription').val(data.Description);
                        $('#modal-add-edit').modal('show');
                        app.stopLoading();

                    },
                    error: function(status) {
                        app.notify('Có lỗi xảy ra', 'error');
                        app.stopLoading();
                    }
                });
            });

        $('#btnSave').on('click',
            function(e) {
                if ($('#frmMaintenance').valid()) {
                    e.preventDefault();
                    var id = $('#hidId').val();
                    var name = $('#txtName').val();
                    var description = $('#txtDescription').val();

                    $.ajax({
                        type: "POST",
                        url: "/Admin/Role/SaveEntity",
                        data: {
                            Id: id,
                            Name: name,
                            Description: description,
                        },
                        dataType: "json",
                        beforeSend: function() {
                            app.startLoading();
                        },
                        success: function(response) {
                            app.notify('Update role successful', 'success');
                            $('#modal-add-edit').modal('hide');
                            resetFormMaintenance();
                            app.stopLoading();
                            loadData(true);
                        },
                        error: function() {
                            app.notify('Has an error', 'error');
                            app.stopLoading();
                        }
                    });
                    return false;
                }

            });

        $('body').on('click',
            '.btn-delete',
            function(e) {
                e.preventDefault();
                var that = $(this).data('id');
                app.confirm('Are you sure to delete?',
                    function() {
                        $.ajax({
                            type: "POST",
                            url: "/Admin/Role/Delete",
                            data: { id: that },
                            beforeSend: function() {
                                app.startLoading();
                            },
                            success: function(response) {
                                app.notify('Delete successful', 'success');
                                app.stopLoading();
                                loadData();
                            },
                            error: function(status) {
                                app.notify('Has an error in deleting progress', 'error');
                                app.stopLoading();
                            }
                        });
                    });
            });


    };


    function resetFormMaintenance() {
        $('#hidId').val('');
        $('#txtName').val('');
        $('#txtDescription').val('');
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/role/GetAllPaging",
            data: {
                keyword: $('#txt-search-keyword').val(),
                page: app.configs.pageIndex,
                pageSize: app.configs.pageSize
            },
            dataType: "json",
            beforeSend: function() {
                app.startLoading();
            },
            success: function(response) {
                var template = $('#table-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $.each(response.Results,
                        function(i, item) {
                            render += Mustache.render(template,
                                {
                                    Name: item.Name,
                                    Id: item.Id,
                                    Description: item.Description
                                });
                        });
                    $("#lbl-total-records").text(response.RowCount);
                    if (render != undefined) {
                        $('#tbl-content').html(render);

                    }
                    wrapPaging(response.RowCount,
                        function() {
                            loadData();
                        },
                        isPageChanged);


                } else {
                    $('#tbl-content').html('');
                }
                app.stopLoading();
            },
            error: function(status) {
                console.log(status);
            }
        });
    };

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
            prev: 'Prev',
            next: 'Next',
            last: 'Last',
            onPageClick: function(event, p) {
                app.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
}