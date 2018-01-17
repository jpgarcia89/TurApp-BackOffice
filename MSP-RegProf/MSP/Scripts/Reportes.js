function InvocarReporte(psReporte, psParametros, EsJuntaMedica, OffLine) {
    //Verificamos que sea diferente de vacio

    debugger
    if (psReporte != '' && psReporte != null) {
        var _vParametros = '';
        if (psParametros != null) {
            var _vParametros = '&ci=' + psParametros.length.toString();
            for (var _i = 0; _i < psParametros.length; _i++) {
                _vParametros += '&v' + _i.toString() + '=' + psParametros[_i][0];
                _vParametros += '&n' + _i.toString() + '=' + psParametros[_i][1];
            }
            if (EsJuntaMedica != null) {
                _vParametros += '&JM=true';
            }
        }
        var _src = GetPathApp('/Reportes/EjecutarReportes.aspx?sr=' + psReporte + _vParametros);
        if (OffLine) {
            _src = GetPathApp('/Reportes/EjecutarReportes.aspx?sr=' + psReporte + _vParametros);
        }

        window.open(_src);
        //$('#frameReportes').attr('src', _src);
        //var _WindowImprimir = $("#wImprimir").data("tWindow");
        //if (_WindowImprimir.isMaximized) {
        //    _WindowImprimir.center().open();
        //}
        //else {
        //    _WindowImprimir.center().maximize().open();
        //}

        //AbrirWaiting();
    }
}


