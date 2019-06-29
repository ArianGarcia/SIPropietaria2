$(document).ready(function () {

    const $tabla_productos = $("#tableProduct");
    const $cliente_Input_busqueda = $("#clientInput");
    const $cliente_select = $("#clientSelect");
    const $producto_Input = $("#productInput");
    const $producto_Select = $("#productSelect");
    const $Agregar_producto_boton = $("#addProduct");
    const $buscar_cliente_boton = $("#searchClient");
    const $buscar_producto_boton = $("#searchProduct");
    const $discountSelect = $("#discountSelect");
    const $SaleState = $("#SaleState");
    const $paymentMethod = $("#paymentMethod");
    const $totalPaid = $("#TotalPaid");
    const $total = $("#total");
    const $commentario = $("#commentary");
    const $boton_venta = $("#salesButton");
    const $modal_comentario = $('#commentaryModal');
    const $billModal = $("#billModal");
    const $successmodalButton = $("#addComentarySucces");
    const $cancelar_comentario = $("#cancelCommentary");
    const $billinfo = $("#billinfo");
    let Objeto_venta;

    $buscar_cliente_boton.click(AgregarEfectoInput);
    $buscar_producto_boton.click(AgregarEfectoInput);



    $cliente_Input_busqueda.keyup(function () {
        const cliente_valor = $cliente_Input_busqueda.val();

        if (cliente_valor.length > 3 || cliente_valor.length === 0) {
            $.ajax({
                type: "GET",
                url: "/Ventas/GetClientes",
                data: { param: cliente_valor },
                success: function (data) {
                    if (data.length === 0 && cliente_valor.length > 3) {
                        $cliente_select.html("<option value>No existen registros que coincidan</option>");
                    } else {
                        $cliente_select.html("");
                        $(data).each(function (index, item) {
                            $cliente_select.append(`<option value="${item.ClienteID}">${item.Nombre}</option>`);
                        });
                        $cliente_select.append(`<option value="">Seleccione un cliente</option>`);
                    }
                }
            });
        }
    });

    $producto_Input.keyup(function () {
        const producto_valor = $producto_Input.val();

        if (producto_valor.length > 3 || producto_valor.length === 0) {

            $.ajax({
                type: "GET",
                url: "/Ventas/GetProductos",
                data: { param: producto_valor },
                success: function (data) {
                    if (data.length === 0 && producto_valor.length > 3) {
                        $producto_Select.html("<option value>No existen productos con ese nombre</option>");
                    } else {
                        $producto_Select.html("");
                        $(data).each(function (index, item) {
                            $producto_Select.append(`<option value="${item.ProductoID}">${item.Nombre}</option>`);
                        });
                        $producto_Select.append(`<option value="">Seleccione un producto</option>`);
                    }
                }
            });
        }
    });



    $Agregar_producto_boton.click(function () {
        const valor_producto_seleccionado = $producto_Select.val();
        if (valor_producto_seleccionado > 0) {
            if (!ProductoRepetido(valor_producto_seleccionado)) {
                $.ajax({
                    type: "GET",
                    url: "/Ventas/GetProducto",
                    data: { id: valor_producto_seleccionado },
                    success: function (data) {

                        if (data !== "0") {

                            CrearElementoTabla(data);

                            Remover_Input_invalido($producto_Select);

                            Actualizar_Precio();

                            CrearArregloProductos();

                            AsignarEventoInputCantidad(data);

                            Remover_producto(data);
                        } else {
                            AgregarInputInvalido($producto_Select, "El producto no existe,refresque el apartado de ventas");
                        }
                    }
                });
            } else {
                AgregarInputInvalido($producto_Select, "El producto ya se encuentra en la lista de productos");
            }
        } else {
            AgregarInputInvalido($producto_Select, "Debe elegir un producto");
        }
    });


    function AgregarEfectoInput() {

        let $btn = $(this);
        let $input = $btn.siblings("input[type='text']");
        let $parentDiv = $btn.parent().parent();
        let icon = $btn.children("i");
        let $select = $btn.parent().siblings("select");

        icon.toggleClass("fa fa-times");
        icon.toggleClass("fa fa-search");
        $input.toggleClass("showIcon");
        $input.val("");
        $input.attr("placeHolder", "Ingrese una palabra");

    }


    function CrearElementoTabla(data) {

        $tabla_productos.append(
            `<tr>
                        <td style="display:none"><input type="hidden" value="${data.ProductoID}" readonly></td>
                        <td><strong name="product">${data.Nombre}</strong></td>
                        <td class="Inputnumber">
                             <input type="number" class="form-control form-control-sm quantity" name="quantity" min="1" max="${data.Cantidad}" value="1">
                             <div class="invalid-feedback">
                             </div>
                        </td>
                        <td><strong name="price">${data.PrecioVenta.toFixed(2)}</strong></td>
                        <td><strong name="subTotal">${data.PrecioVenta.toFixed(2)}</strong></td>
                        <td><button type="button" class="btn btn-danger btn-sm"><i class="fa fa-times" aria-hidden="true"></i></button></td>
                     </tr>`
        );
    }


    function CrearArregloIdProductos() {
        const productos = Array.from($tabla_productos
            .children("tr")
            .children("td")
            .children("input[type='hidden']"));
        return productos;
    }


    function ProductoRepetido(data) {

        let productos = CrearArregloIdProductos();
        let productoRepetido = false;

        $(productos).each(function (index, item) {
            if (parseInt($(item).val()) === parseInt(data)) {
                productoRepetido = true;
            }
        });
        return productoRepetido;
    }



    function Actualizar_Precio() {

        let price = 0;
        let ProductoPrecio = Array.from($tabla_productos.children("tr").children("td")
            .children("strong[name='subTotal']"));

        $(ProductoPrecio).each(function (i, priceItem) {
            price += parseFloat($(priceItem).html());
        });

        $total.html(price.toFixed(2));
    }




    function AsignarEventoInputCantidad(data) {

        let Productquantity = $tabla_productos.children("tr").children("td").children("input[name='quantity']");
        
        $(Productquantity).last().on("keyup click", data, function () {
            let element = this;
            CalcularSubTotalProducto(element, data);
            Actualizar_Precio();
        });

    }


    function CalcularSubTotalProducto(element, data) {

        let subTotal = $(element).parent().parent().children("td").children("strong[name='subTotal']");
        let $cantidad_input = $(element).parent().parent().children("td").children("input[name='quantity']");
        let valor_input_cantidad = $cantidad_input.val();
        let total_precio_producto;

        if (valor_input_cantidad.length === 0) {
            AgregarInputInvalido($cantidad_input, "El campo cantidad no puede estar vacio.");
            $boton_venta.attr("disabled", true);
        }else if (isNaN(parseInt(valor_input_cantidad))) {
            AgregarInputInvalido($cantidad_input, "El campo cantidad debe ser un valor numerico.");
            $boton_venta.attr("disabled", true);
        } else if (parseInt(valor_input_cantidad) > parseInt($cantidad_input.attr("max"))) {
            AgregarInputInvalido($cantidad_input, "Ha excedido la cantidad disponible de este producto");
            $boton_venta.attr("disabled", true);
        } else if (parseInt(valor_input_cantidad) <= 0) {
            AgregarInputInvalido($cantidad_input, "El valor debe ser mayor a 0");
            $boton_venta.attr("disabled", true);
        }else {
            Remover_Input_invalido($cantidad_input);
            $boton_venta.attr("disabled", false);
            total_precio_producto = data.PrecioVenta * valor_input_cantidad;
            $(subTotal).html(total_precio_producto.toFixed(2));
        }
    }


    function Remover_producto(data) {

        let $deleteButton = $tabla_productos.children("tr").children("td").children("button");
        let $element;

        $deleteButton.last().on("click", data, function (i, item) {
            $element = $(this);
            $element.parent().parent().remove();
            Actualizar_Precio();
        });
    }

    function CrearObjetoVenta() {

        let cliente_seleccionado = $cliente_select.val();

        let Venta = {
            Productos: CrearArregloProductos(),
            Comentario: $commentario.val(),
            Total: CalcularPrecioTotal(),
            ClienteID: parseInt(cliente_seleccionado) > 0 ? parseInt(cliente_seleccionado) : 0
        };
        return Venta;
    }

    function CalcularPrecioTotal() {
        let $precio_productos = $tabla_productos.children("tr").children("td").children("strong[name='price']");
        let cantidad_inputs;
        let precio = 0;

        $precio_productos.each(function (index, element) {
            cantidad_inputs = parseFloat($(element).parent().parent().children("td").children("input[name='quantity']").val());
            precio += parseFloat($(element).html()) * cantidad_inputs;
        });
        return precio;
    }


    $boton_venta.click(function () {

        Objeto_venta = CrearObjetoVenta();

        Remover_Input_invalido($producto_Select);

        if (Objeto_venta.Productos.length === 0) {
            AgregarInputInvalido($producto_Select, "Debe introducir al menos un producto para realizar la venta");
        } else if (Objeto_venta.ClienteID === 0) {
            AgregarInputInvalido($cliente_select, "Debe elegir un cliente");
        }else {
            $modal_comentario.modal('show');
        }
    });

    function CambiarEstadoInvalido(element, message, addmessage = true) {
        let $e = $(element);
        if ($e.val().length > 0) {
            Remover_Input_invalido($e);
        } else {
            AgregarInputInvalido($e, message, addmessage);
            $e.focus();
        }
    }

    function AgregarInputInvalido(element, message, addMessage = true) {
        let $e = $(element);
        $e.focus();
        $e.addClass("is-invalid");
        if (addMessage) {
            $e.siblings(".invalid-feedback").html(message);
        }
    }

    function Remover_Input_invalido(element) {
        let $e = $(element);
        $e.removeClass("is-invalid");
    }

    function CrearArregloProductos() {

        let productsTr = $tabla_productos.children("tr");
        let quantityError = 0;
        let discountError = 0;
        let producto_arreglo = [];
        let emptyProductArry = [];

        $(productsTr).each(function (i, item) {
            let tableTds = Array.from($(item).children("td"));
            let modelo_producto = {
                ProductoID: parseInt($(tableTds).children("input[type='hidden']").val()),
                Nombre: $(tableTds).children("strong[name='product']").html(),
                Cantidad: parseInt($(tableTds).children("input[name='quantity']").val()),
                SubTotal: parseFloat($(tableTds).children("strong[name='subTotal']").html()),
                PrecioUnitario: parseFloat($(tableTds).children("strong[name='price']").html())
            };
            if (isNaN(modelo_producto.Cantidad)) {
                quantityError += 1;
            }
            producto_arreglo.push(modelo_producto);
        });
        if (quantityError > 0) {
            bootbox.alert("El campo cantidad de algun producto esta vacio, por favor llene el campo");
            return emptyProductArry;
        }else {
            return producto_arreglo;
        }
    }

    $producto_Select.click(function () {
        Remover_Input_invalido(this);
    });
    
    function EnviarObjetoVenta(venta) {

        bootbox.confirm("Esta seguro de realizar esta venta?", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Ventas/Create",
                    dataType: "json",
                    data: { data: JSON.stringify(venta) },
                    success: function (data) {
                        if (data.success === "1") {
                            bootbox.alert("La venta se ha realizado correctamente", function () {
                                window.location.replace("/Ventas");
                            });
                        } else {
                            bootbox.alert("Error: no se ha realizado la venta");
                        }
                    },
                    error: function (response) {
                        bootbox.alert("Error: no se ha realizado la venta");
                    }
                });
            }
        });
    }


    $successmodalButton.click(function () {
        Objeto_venta.Comentario = $commentario.val();
        $modal_comentario.modal('hide');
        EnviarObjetoVenta(Objeto_venta);
    });
    $cancelar_comentario.click(function () {
        Objeto_venta.Comentario = "";
        $modal_comentario.modal('hide');
        EnviarObjetoVenta(Objeto_venta);
    });

});

