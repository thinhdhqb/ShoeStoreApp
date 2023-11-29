$('#select-address').change(function () {
    if ($(this).val() === "new") {
        $("#form-new-address").removeClass("d-none");
        $("#btn-order").addClass("d-none");
    }
    else {
        $("#form-new-address").addClass("d-none");
        $("#btn-order").removeClass("d-none");
        $("#input-address-id").val($(this).val());
    }
})

$(document).ready(() => {
    $("#input-address-id").val($("option:selected").val());
})