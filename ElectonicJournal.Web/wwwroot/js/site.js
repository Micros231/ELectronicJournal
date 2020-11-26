jQuery(function ($)
{
    $('.datepickerMonth').datepicker({
        format: "dd.mm.yyyy",
        startView: "months",
        minViewMode: "months",
        language: "ru"
    });
    $('.datepicker').datepicker({
        format: "dd.mm.yyyy",
        language: "ru"
    });
}); 