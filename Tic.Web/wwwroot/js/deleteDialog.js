$(document).on('click', '#deleteItem', function (e) {
    item_to_delete = e.currentTarget.dataset.id;
    $('#option_Nombre').html("&nbsp;<i class=\"bi bi-question-circle \" style=\"font-size: 30px;\"></i> &nbsp; ¡ELIMINAR REGISTRO!");
    $('#option_Descripcion1').html("&nbsp;¡ATENCIÓN!");
    $('#option_Descripcion2').html("&nbsp;¡Realmente Desea Eliminar el Registro?");
    $('#opcion_accion').html(`<a href=\"${itemdelete}${item_to_delete}\"  class=\"btn btn-outline-danger btn-bor\"><i class=\"bi bi-trash\" style=\"font-size: 20px;\"></i> Borrar</a>`);
    $('#Option_Modal').modal('show');
});