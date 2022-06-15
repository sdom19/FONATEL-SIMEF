    function setItemSelected () {

        var button= $(this);        
        var dataSeleted = button.attr("data-json-selected");
        dataSeleted = dataSeleted.replace(/(\r?\n|\r)/g, " ");
        dataSeleted = dataSeleted.trim();
        dataSeleted = '{"data":[' + dataSeleted + ']}';
        dataSeleted = JSON.parse(dataSeleted);
        var etiHTML = null;
        var etiHTMLValue = null;

        $.each(dataSeleted.data[0], function (key, value) {
            
            etiHTML = $("#" + $.trim(key));
            etiHTMLValue = $.trim(value);            

            if (etiHTML.attr('type') == 'checkbox' || etiHTML.attr('type') == 'radio')
            {
                etiHTML.prop('checked', etiHTMLValue);
            }          

            if (etiHTML.attr('type') == 'text' || etiHTML.attr('type') == 'hidden' || etiHTML.attr('type') == 'number')
            {
                etiHTML.val(etiHTMLValue);
            }

            if (etiHTML.is('div')) {
                etiHTML.empty();
                etiHTML.append(etiHTMLValue);                
            }
            
            if (etiHTML.is('select') || etiHTML.is('textarea')) {
                etiHTML.val(etiHTMLValue);


            }            
        });        
    };
