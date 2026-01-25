$(function(){
    listarClientes();
    $('#btnNovoClient').click(function(){
        limparFormClient();
        $('#formTituloClient').text('Novo cliente');
        $('#tblClients').hide();
        $('#formClient').show();
    });
});

function listarClientes() {
    $.get('/api/clients', function(data) {
        var html = '';
        data.forEach(function(c){
            html += '<tr><td>'+c.ClientId+'</td><td>'+c.Name+'</td>'+
                '<td><button onclick="editarCliente(\''+c.Id['$oid']+'\')">Editar</button> '+
                '<button onclick="removerCliente(\''+c.Id['$oid']+'\')">Remover</button></td></tr>';
        });
        $('#tblClients tbody').html(html);
    });
}

function salvarCliente(){
    var client = {
        ClientId: $('#clientId').val(),
        ClientSecret: $('#clientSecret').val(),
        Name: $('#clientName').val()
    };
    var id = $('#clientIdHidden').val();
    if(id){
        $.ajax({url:'/api/clients/'+id, type:'PUT', data:JSON.stringify(client), contentType:'application/json', success:function(){depoisSalvarClient();}});
    }else{
        $.ajax({url:'/api/clients', type:'POST', data:JSON.stringify(client), contentType:'application/json', success:function(){depoisSalvarClient();}});
    }
    return false;
}
function editarCliente(id){
    $.get('/api/clients/'+id, function(c){
        limparFormClient();
        $('#clientIdHidden').val(id);
        $('#clientId').val(c.ClientId);
        $('#clientSecret').val(c.ClientSecret);
        $('#clientName').val(c.Name);
        $('#formTituloClient').text('Editar cliente');
        $('#tblClients').hide();
        $('#formClient').show();
    });
}
function removerCliente(id){
    if(confirm('Remover este cliente?')){
        $.ajax({url:'/api/clients/'+id, type:'DELETE', success:listarClientes});
    }
}
function depoisSalvarClient(){
    listarClientes();
    $('#formClient').hide();
    $('#tblClients').show();
}
function limparFormClient(){
    $('#clientIdHidden').val('');
    $('#clientId').val('');
    $('#clientSecret').val('');
    $('#clientName').val('');
}

