var editingTables = {};
$(document).ready(function () {
    $("input.sensitive-data").val("");
    window.alert = function (title, message, type) {
        if (type == null || type == 'undefined' || type === '')
            type = 'warning';
        swal({ title: title, text: message, confirmButtonText: "Tamam", allowOutsideClick: true, html: true, type: type, animation: "slide-from-top" });
    };
});

var logStr = function logStr(string) {
    if (window.console) {
        // IE...
        window.console.log('SweetAlert: ' + string);
    }
};

$(function () {
    $('#menu-toggle').toggle(
        function () {
            if ($('#wrapper').hasClass('right-sidebar')) {
                $('body').addClass('right-side-collapsed');
                $('.navbar-header').addClass('logo-collapsed');
                $('#sidebar .slimScrollDiv').css('overflow', 'initial');
                $('#sidebar .menu-scroll').css('overflow', 'initial');
            } else {
                $('body').addClass('left-side-collapsed');
                $('.navbar-header').addClass('logo-collapsed');
                $('#sidebar .slimScrollDiv').css('overflow', 'initial');
                $('#sidebar .menu-scroll').css('overflow', 'initial');
            }
        }, function () {
            if ($('#wrapper').hasClass('right-sidebar')) {
                $('body').removeClass('right-side-collapsed');
                $('.navbar-header').removeClass('logo-collapsed');
                $('#sidebar .slimScrollDiv').css('overflow', 'hidden');
                $('#sidebar .menu-scroll').css('overflow', 'hidden');
            } else {
                $('body').removeClass('left-side-collapsed');
                $('.navbar-header').removeClass('logo-collapsed');
                $('#sidebar .slimScrollDiv').css('overflow', 'hidden');
                $('#sidebar .menu-scroll').css('overflow', 'hidden');
            }
        }
    );

    if ($('#wrapper').hasClass('right-sidebar')) {
        $('ul#side-menu li').hover(function () {
            if ($('body').hasClass('right-side-collapsed')) {
                $(this).addClass('nav-hover');
            }
        }, function () {
            if ($('body').hasClass('right-side-collapsed')) {
                $(this).removeClass('nav-hover');
            }
        });
    } else {
        $('ul#side-menu li').hover(function () {
            if ($('body').hasClass('left-side-collapsed')) {
                $(this).addClass('nav-hover');
            }
        }, function () {
            if ($('body').hasClass('left-side-collapsed')) {
                $(this).removeClass('nav-hover');
            }
        });
    }

    var heightDocs = $(window).height() - 70;
    var window_h = $(window).height() + 50;
    var content_h = $('#page-wrapper').height();
    if (window_h <= content_h) {
        //$('.page-footer').css('position', 'relative');
        $('#page-wrapper').css('min-height', heightDocs);
    } else {
        //$('.page-footer').css('position', 'absolute');
        $('#sidebar').css('height', heightDocs);
        $('#page-wrapper').css('height', window_h);
    }

    $('#side-menu').metisMenu();

    $(window).bind("load resize", function () {
        if ($(this).width() < 768) {
            $('body').removeClass('left-side-collapsed');
            $('.navbar-header').removeClass('logo-collapsed');
            $('div.sidebar-collapse').addClass('collapse');
            $('#sidebar').css('height', 'auto');
        } else {
            $('div.sidebar-collapse').removeClass('collapse');
            $('#sidebar').css('height', 'auto');
        }
    });
    var page_wrapper_h = $('#page-wrapper').height();
    var menu_h = $('#side-menu').height();
    var sidebar_h;
    if (page_wrapper_h < menu_h) {
        sidebar_h = page_wrapper_h;
        $('.menu-scroll').slimScroll({ "height": sidebar_h, "wheelStep": 5 });
    }

    $("[data-toggle='tooltip'], [data-hover='tooltip']").tooltip();
    $("[data-toggle='popover'], [data-hover='popover']").popover();
    //$('.btn-fullscreen').click(function () { $.Arwend.FullScreen(); });

    $(".portlet").each(function (index, element) {
        var me = $(this);
        $(">.portlet-header>.tools>i", me).click(function (e) {
            if ($(this).hasClass('fa-chevron-up')) {
                $(">.portlet-body", me).slideUp('fast');
                $(this).removeClass('fa-chevron-up').addClass('fa-chevron-down');
            }
            else if ($(this).hasClass('fa-chevron-down')) {
                $(">.portlet-body", me).slideDown('fast');
                $(this).removeClass('fa-chevron-down').addClass('fa-chevron-up');
            }
            else if ($(this).hasClass('fa-cog')) {
                //Show modal
            }
            else if ($(this).hasClass('fa-refresh')) {
                //$(">.portlet-body", me).hide();
                $(">.portlet-body", me).addClass('wait');

                setTimeout(function () {
                    $(">.portlet-body", me).removeClass('wait');
                }, 1000);
            }
            else if ($(this).hasClass('fa-times')) {
                me.remove();
            }
        });
    });
    window.onload = function () { date() }, setInterval(function () { date() }, 1000);
    function date() {
        var dt = new Date();
        var day = dt.getDay();
        var mm, dd, h, m, s;
        mm = (mm = dt.getMonth() + 1) < 10 ? '0' + mm : mm
        dd = (dd = dt.getDate()) < 10 ? '0' + dd : dd
        h = (h = dt.getHours()) < 10 ? '0' + h : h
        m = (m = dt.getMinutes()) < 10 ? '0' + m : m
        s = (s = dt.getSeconds()) < 10 ? '0' + s : s
        var days = new Array("Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi");
        $('.get-date').html(days[day] + ', ' + dd + '.' + mm + '.' + dt.getFullYear());
        $('#getHours').html(h);
        $('#getMinutes').html(m);
        $('#getSeconds').html(s);
    }
    $(window).scroll(function () { if ($(this).scrollTop() < 200) { $('#totop').fadeOut(); } else { $('#totop').fadeIn(); } });
    $('#totop').on('click', function () { $('html, body').animate({ scrollTop: 0 }, 'fast'); return false; });

    $("input[type='file'].with-preview").change(function () {
        var preview = document.getElementById($(this).attr("data-preview"));
        var fileInput = $(this);
        var fileReader = new FileReader();
        fileReader.onload = function (e) {
            $(preview).attr("src", e.target.result);
        };
        fileReader.readAsDataURL(this.files[0]);
    });

    $("input[type='file'].no-preview").change(function () {
        $(".filename").html(this.files[0].name);
    });

    $('.dropdown-menu a[data-toggle="popover"]').click(function (e) {
        e.stopPropagation();
    });

    $('body').on('click', function (e) {
        $('.dropdown-menu a[data-toggle="popover"]').each(function () {
            if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                $(this).popover('hide');
            }
            else {
                e.stopPropagation();
            }
        });
    });
});

$(document).ready(function () {
    if (typeof addProducts == 'undefined') {
        $('table.table tbody').on('click', 'tr', function (e) {

            var id = $(this).data('id');
            var datatype = $(this).data('type');
            var deleteSubItems = (datatype == 'Menu' || datatype == 'Showcase');
            var currentRow = $(this)[0];
            if (e.target) {
                if ($(e.target.parentElement).hasClass('btn-group'))
                    $("button[data-toggle=dropdown]", this).dropdown('toggle'); e.stopPropagation();
                if ($(e.target).hasClass('switch-small') || $(e.target.parentElement).hasClass('btn-group')) return;
                if (e.target.tagName == 'A' && $(e.target).hasClass('delete')) {
                    $.BSM.Business.Base.DeleteItem(datatype, id, function () { editingTables[datatype].fnDeleteRow(currentRow); }, deleteSubItems);
                }
                else {
                    e.stopPropagation();
                    var editurl = $(this).closest('table').data('editurl');
                    if (editurl.length > 0) {
                        if (id != null && id != 'undefined')
                            window.open(editurl + '?id=' + id, "_blank");
                    }
                }
            }
        });    
    }
    

});
(function ($) {
    $.fn.BSM = function () {
        return $.BSM;
    };
    $.BSM = function () {

    };
    $.BSM.Initialize = function () {
        $.BSM.ReInitialize();
    };
    $.BSM.ReInitialize = function () {
        $(document).ready(function () {
            $(document).unbind("keyup");

            $(document).keyup(function (event) { if (event.keyCode === 27) $.BSM.Dialogs.Close(); });
        });
    }
    $.BSM.Settings = {

    };
    $.BSM.Pages = {
        Login: { ID: 0, Url: "/Account/Login" },
        ForgotPassword: { ID: 1, Url: "/Account/ForgotPassword" }
    };
    $.BSM.GetActionsColumn = function (entity, allowEdit) {
        if (allowEdit == null || allowEdit == 'undefined') allowEdit = false;
        var builder = "";
        builder += "<div class='btn-group'>";
        builder += "<button class='btn btn-primary btn-sm' type='button'>İşlemler</button>";
        builder += "<button class='btn btn-primary btn-sm dropdown-toggle' data-toggle='dropdown' type='button'>";
        builder += "<span class='caret'></span><span class='sr-only'>Toggle Dropdown</span>";
        builder += "</button>";
        builder += "<ul class='dropdown-menu pull-right' role='menu'>";
        if (allowEdit)
            builder += "<li><a href='javascript:;' class='edit'><i class='fa fa-edit'></i>&nbsp;Düzenle</a></li>";
        builder += "<li><a href='javascript:;' class='delete'><i class='fa fa-trash-o'></i>&nbsp;Sil</a></li>";
        if (entity != null) {
            builder += "<li class='divider'></li>";
            builder += "<li><a data-width='600px' data-container='body' data-toggle='popover' data-placement='left' data-html='true'";
            builder += " data-content='<b>Güncelleyen : </b>";
            builder += entity.UserModified;
            builder += "<br/><b>Güncel. Tarihi : </b>";
            builder += entity.DateModified;
            builder += "<br/><b>Oluşturan : </b>";
            builder += entity.UserCreated;
            builder += "<br/><b>Oluşt. Tarihi : </b>";
            builder += entity.DateCreated;
            builder += "' class='text-center' href='javascript:;'>Güncelleme Bilgisi</a></li>";
        }
        builder += "</ul>";
        builder += "</div>";
        return builder;
    },
    $.BSM.Business = {
        Base: {
            DeleteItem: function (dataType, id, callback, deleteSubItems) {
                if (deleteSubItems == null || deleteSubItems == 'undefined')
                    deleteSubItems = false;
                var subText = "Silme işlemini onaylarsanız bu kayıt kalıcı olarak silinecektir!";
                if (deleteSubItems)
                    subText += "<br /><div class='squaredFour' id='delete-subitems'><input type='checkbox' name='DeleteSubItems' id='DeleteSubItems' /><label for='DeleteSubItems'><span>Tüm alt öğeleriyle birlikte sil</span></label>";
                swal({
                    title: "Silmek istediğinize emin misiniz?",
                    text: subText,
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Evet",
                    cancelButtonText: "Hayır",
                    closeOnConfirm: false,
                    closeOnCancel: true,
                    html: true
                },
                function (isConfirm) {
                    if (isConfirm) {
                        $.Arwend.Ajax.Parameters = {
                            Action: "deleteitem",
                            DataType: dataType,
                            ID: id,
                            DeleteSubItems: $("#DeleteSubItems").is(':checked')
                        };
                        $.Arwend.Ajax.GetResponse(function (response) {
                            if (response.message.isNullOrEmpty()) {
                                swal({ title: "Silindi!", text: "Silme işlemi başarıyla gerçekleştirildi.", type: "success", allowOutsideClick: true });
                                callback();

                            }
                            else
                                swal("Hata", response.message, "error");
                        });
                    }
                });
            }
        },
        Content: {
        },
        Configuration: {

        },
        Validation: {
            IsEmailExists: function (email) {
                var result = false;
                $.BSM.Ajax.Parameters = {
                    Protocol: window.location.protocol,
                    Action: "IsEmailExists",
                    email: email
                };
                if ($.BSM.Ajax.IsCompleted) {
                    $.BSM.Ajax.IsCompleted = false;
                    $.BSM.Ajax.GetResponse(function (Response) {
                        result = Response.Result;
                    })
                };
                return result;
            }
        }
    }
    $.BSM.Initialize();
}(jQuery));