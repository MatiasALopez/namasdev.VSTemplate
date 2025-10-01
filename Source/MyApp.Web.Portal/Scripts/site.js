var myApp = function () {

    function init() {
        initNamasdevJS();
        initMenus();
        initMainWrapperHeightSize();
    }

    function setActiveMenuOption(name) {
        $(nmd.utils.stringFormat('.menu-{0}', name)).addClass('active');
    }

    function showErrorModal(message, callback) {
        nmd.ui.controls.showErrorModal(message || 'Ha ocurrido un error. Por favor, intente nuevamente mas tarde.', callback);
    }

    function postJson(url, data, successCallback, options) {
        var optsDefaults = {
            showLoading: true,
            showLoadingSelector: null,
            confirm: false,
            confirmTitle: null,
            confirmMessage: null,
            confirmOptions: null,
        }

        var opts = $.extend({}, optsDefaults, options);

        if (opts.confirm) {
            nmd.ui.controls.showConfirmModal(
                opts.confirmTitle,
                opts.confirmMessage,
                function (confirmation) {
                    if (confirmation) {
                        post();
                    }
                },
                opts.confirmOptions
            );
        } else {
            post();
        }

        function post() {
            if (opts.showLoading) {
                nmd.ui.controls.showLoading(opts.showLoadingSelector);
            }

            $.post(url, data,
                function (response) {
                    try {
                        if (successCallback) {
                            successCallback(response);
                        }
                    } catch (e) {
                        if (opts.showLoading) {
                            nmd.ui.controls.hideLoading(opts.showLoadingSelector);
                        }

                        console.error(e);
                    }
                },
                'json')
                .fail(function () {
                    if (opts.failCallback) {
                        opts.failCallback();
                    } else {
                        myApp.showErrorModal();
                    }
                })
                .always(function () {
                    if (opts.showLoading) {
                        nmd.ui.controls.hideLoading(opts.showLoadingSelector);
                    }

                    if (opts.alwaysCallback) {
                        opts.alwaysCallback();
                    }
                });
        }
    }

    //---

    function initNamasdevJS() {
        nmd.ui.forms.initDisableOnSubmitButtons();
        nmd.ui.forms.setValidationSummaryVisibility();

        nmd.ui.controls.initAutopostback();
        nmd.ui.controls.initPrintButton();
        nmd.ui.controls.initScrollFocus();
        nmd.ui.controls.initInputFileButtons();
        nmd.ui.controls.initInputFiltering();
        nmd.ui.controls.initClickControls();
        nmd.ui.controls.initToggleStateControls();
        nmd.ui.controls.initBootstrapTooltips();
        nmd.ui.controls.initBootstrapCustomFileInput();
        nmd.ui.controls.initCheckBoxSelection();
        nmd.ui.controls.initTableCheckBoxSelection();
        nmd.ui.controls.initTableSorting();
        nmd.ui.controls.initTableOrderPOST();
        nmd.ui.controls.initModals();
        nmd.ui.controls.initIframeModalsLinks();
        nmd.ui.controls.initMultiSelect();
        nmd.ui.controls.initSelectpickerAjax();
        nmd.ui.controls.initDatePicker();
        nmd.ui.controls.initCharacterCounter();
        nmd.ui.controls.initNumeric();
        nmd.ui.controls.initNumericInteger();
        nmd.ui.controls.initInputMaskNumeric();
        nmd.ui.controls.initInputMaskInteger();
    }

    function initMenus() {
        $('.submenu-categoria').each(function () {
            var $this = $(this);
            if ($this.find('li').length == 0) {
                $this.closest('.submenu-categoria-container').remove();
            }
        });

        if ($('#submenuReportes li').length == 0) {
            $('#menuReportes').remove();
        }

        if ($('#submenuFunciones li').length == 0) {
            $('#menuFunciones').remove();
        }
    }

    function initMainWrapperHeightSize() {
        setMainWrapperHeightSize();

        $(window).resize(function () {
            setMainWrapperHeightSize();
        });

        function setMainWrapperHeightSize() {
            var windowHeight = $(window).height() - ($('#navTopMenu').height() + 32) - 10;
            var mainContentHeight = $('#mainContent').height() + 20;
            var height = Math.max(windowHeight, mainContentHeight);
            $('#mainWrapper').css('min-height', height);
            $('.iframe-main-content').css('min-height', height - 5);
        }
    }

    return {
        init,
        setActiveMenuOption,
        showErrorModal,
        postJson
    };
}();
