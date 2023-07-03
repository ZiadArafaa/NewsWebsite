$(document).ready(function () {
    $('.js-render-modal').click(function () {
        var btn = $(this);
        var modal = $("#Modal");
        modal.find('#ModalLabel').text(btn.data('title'));
        $.get({
            url: btn.data('url'),
            success: function (data) {
                modal.find('.modal-body').html(data);
                $.validator.unobtrusive.parse(modal);
            }
        });
        modal.modal('show');

    });
});