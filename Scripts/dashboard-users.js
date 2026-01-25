$(function(){
    listarUsuarios();
    $('#btnNovoUsuario').click(function(){
        limparForm();
        $('#formTitulo').text('Novo usuário');
        $('#tblUsuarios').hide();
        $('#formUsuario').show();
    });
});

function listarUsuarios() {
    $.get('/api/users', function(data) {
        var html = '';
        data.forEach(function(u){
            html += '<tr><td>'+u.UserName+'</td><td>'+u.Role+'</td>'+
                '<td><button onclick="editarUsuario(\''+u.Id['$oid']+'\')">Editar</button> '+
                '<button onclick="removerUsuario(\''+u.Id['$oid']+'\')">Remover</button></td></tr>';
        });
        $('#tblUsuarios tbody').html(html);
    });
}

function salvarUsuario(){
    var usuario = {
        UserName: $('#usuarioNome').val(),
        PasswordHash: $('#usuarioSenha').val(),
        Role: $('#usuarioRole').val()
    };
    var id = $('#usuarioId').val();
    if(id){
        $.ajax({url:'/api/users/'+id, type:'PUT', data:JSON.stringify(usuario), contentType:'application/json', success:function(){depoisSalvar();}});
    }else{
        $.ajax({url:'/api/users', type:'POST', data:JSON.stringify(usuario), contentType:'application/json', success:function(){depoisSalvar();}});
    }
    return false;
}
function editarUsuario(id){
    $.get('/api/users/'+id, function(u){
        limparForm();
        $('#usuarioId').val(id);
        $('#usuarioNome').val(u.UserName);
        $('#usuarioRole').val(u.Role);
        $('#formTitulo').text('Editar usuário');
        $('#tblUsuarios').hide();
        $('#formUsuario').show();
    });
}
function removerUsuario(id){
    if(confirm('Remover este usuário?')){
        $.ajax({url:'/api/users/'+id, type:'DELETE', success:listarUsuarios});
    }
}
function depoisSalvar(){
    listarUsuarios();
    $('#formUsuario').hide();
    $('#tblUsuarios').show();
}
function limparForm(){
    $('#usuarioId').val('');
    $('#usuarioNome').val('');
    $('#usuarioSenha').val('');
    $('#usuarioRole').val('');
}

