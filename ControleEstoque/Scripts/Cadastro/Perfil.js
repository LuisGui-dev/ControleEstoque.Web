function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#cbx_ativo').prop('checked', dados.Ativo);
    
    var lista_usuario = $('#lista_usuario');
    lista_usuario.find('input[type=checkbox]').prop('checked', false);

    if (dados.Usuarios) {
        for (var i = 0; i < dados.Usuarios.length; i++) {
            var usuario = dados.Usuarios[i];
            var cbx = lista_usuario.find('input[data-id-usuario=' + usuario.Id + ']');
            if (cbx) {
                cbx.prop('checked', true);
            }
        }
    }
}

function set_focus_form() {
    $('#txt_nome').focus()
}

function get_dados_incluir() {
    return {Id: 0, Nome: '', Ativo: true};
}


function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        Ativo: $('#cbx_ativo').prop('checked'),
        idUsuarios: get_lista_usuarios_marcados()
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Ativo ? 'Sim' : 'Não');
}

function get_lista_usuarios_marcados() {
    var ids = [];
    var lista_usuario = $('#lista_usuario');
    lista_usuario.find('input[type=checkbox]').each(function (index, input) {
        var cbx = $(input),
            marcado = cbx.is(':checked');
            
        if(marcado) {
            ids.push(parseInt(cbx.attr('data-id-usuario')));
        }
    });
    
    return ids;
}

$(document).ready(function () {
    var grid = $('#grid_cadastro > tbody');
    for (var i = 0; i < linhas.length; i++) {
        grid.append(criar_linha_grid(linhas[i]));
    }
});