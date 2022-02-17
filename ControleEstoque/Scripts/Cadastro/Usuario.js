﻿function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_login').val(dados.Login);
    $('#txt_senha').val(dados.Senha);
}

function set_focus_form() {
    $('#txt_nome').focus()
}

function get_dados_incluir() {
    return {
        Id: 0,
        Nome: '',
        Login: '',
        Senha: ''
    };
}


function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        Login: $('#txt_login').val(),
        Senha: $('#txt_senha').val()
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Login);
}

$(document).ready(function () {
    var grid = $('#grid_cadastro > tbody');
    for (var i = 0; i < linhas.length; i++) {
        grid.append(criar_linha_grid(linhas[i]));
    }
});