ko.bindingHandlers.loading = {
    init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {

      var opts = {
		  lines: 12, // The number of lines to draw
		  length: 7, // The length of each line
		  width: 5, // The line thickness
		  radius: 10, // The radius of the inner circle
		  corners: 1, // Corner roundness (0..1)
		  rotate: 0, // The rotation offset
		  direction: 1, // 1: clockwise, -1: counterclockwise
		  color: '#000', // #rgb or #rrggbb or array of colors
		  speed: 1, // Rounds per second
		  trail: 100, // Afterglow percentage
		  shadow: true, // Whether to render a shadow
		  hwaccel: false, // Whether to use hardware acceleration
		  className: 'spinner', // The CSS class to assign to the spinner
		  zIndex: 2e9, // The z-index (defaults to 2000000000)
		  top: '50%', // Top position relative to parent
		  left: '50%' // Left position relative to parent
		};
		var spinner = new Spinner(opts);
          spinner.spin(element);
    },
      update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
          
    }
};


ko.bindingHandlers.bootstrapSwitch = {
    init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
		$(element).bootstrapSwitch({
			onSwitchChange: function(){
				if ($('form').length > 0 && $(element).parents('.has-error').length > 0){
					$('form').validate().element($(element));
				}
			}
		});
    },
    update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
    	var obserable = valueAccessor();
    	if (obserable()){
    		$(element).bootstrapSwitch('state', true);
    	}
    	else{
    		$(element).bootstrapSwitch('state', false);
    	}
		
    }
     
};


ko.bindingHandlers.monthpicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
    	$(element).datepicker( {
    	    format: "yyyy-mm",
    	    viewMode: "months", 
    	    minViewMode: "months",
    	    autoclose: true
    	}).on("hide", function (ev) {
            var observable = valueAccessor();
            observable(ev.date);
        });
    	
    	 $(element).parent().find(".glyphicon-calendar").click(function(){
     		$(element).trigger('focus')
     	})
    	
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        $(element).datepicker("update", value);
    }
}; 
	
ko.bindingHandlers.select2 = {
	init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
		$(element).select2();
    },
    update: function (element, valueAccessor) {
    	var observable = valueAccessor();	    	
    	$(element).select2("val",observable());
    }
}
ko.bindingHandlers.select2_noSearch = {
	init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
		$(element).select2({
		    minimumResultsForSearch: -1
		});
    },
    update: function (element, valueAccessor) {
    	var observable = valueAccessor();
    	$(element).select2("val",observable());
    }
}

ko.bindingHandlers.uniform = {
	init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
		$(element).uniform();
    }
}
ko.bindingHandlers.upperCase= {
	init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
		var observable = valueAccessor();
		if (observable() != null)
		$(element).val(observable().toUpperCase())
		
    },
    update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
    	var observable = valueAccessor();
    	if (observable() != null)
		$(element).val(observable().toUpperCase())
    }	
}
ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        $(element).datepicker({format:'yyyy-mm-dd', autoclose: true}).on("hide", function (ev) {
            var observable = valueAccessor();
            observable(ev.date);
        });
        $(element).parent().find(".glyphicon-calendar").click(function(){
    		$(element).trigger('focus')
    	})
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        $(element).datepicker("update", value);
    }
}; 

ko.bindingHandlers.requiredRule = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        $(element).rules('add',{
        	required: true
        });
    }	
}

ko.bindingHandlers.numberRule = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        $(element).rules('add',{
        	number: true
        });
    }	
}


ko.bindingHandlers.gtZeroRule = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        $(element).rules('add',{
        	min: 0
        });
    }	
}

