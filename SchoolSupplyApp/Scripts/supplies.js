$(function () {
    $("table").on("click", ".edit", function () {
        var name = $(this).data("name");
        var id = $(this).data("id");
        $("#form").prop("action", "/admin/updatesupply");
        $("#form-title").text("Edit " + name);
        $("#supply-name").val(name);
        $("#supply-id").val(id);
        $("#form-submit").removeClass("btn-success");
        $("#form-submit").addClass("btn-warning");
        $("#form-submit").text("Update");
    });
});