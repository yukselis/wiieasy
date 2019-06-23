(function ($) {
    if (!$.exists) {
        $.extend({
            exists: function (element) {
                if (typeof element == null) return false;
                if (typeof element != "object") element = $(element);
                return element.length ? true : false;
            },
            invisible: function (elements) {
                return elements.each(function () {
                    $(this).css("visibility", "hidden");
                });
            },
            visible: function (elements) {
                return elements.each(function () {
                    $(this).css("visibility", "visible");
                });
            }
        });
        $.fn.extend({
            exists: function () {
                return $.exists($(this));
            },
            invisible: function () {
                return $.invisible(this);
            },
            visible: function () {
                return $.visible(this);
            }
        });
    }
})(jQuery);
(function ($) {
    $.fn.Arwend = function () {
        return $.Arwend;
    };
    $.Arwend = function () {

    };
    $.Arwend.Initialize = function () {
        $.Arwend.ReInitialize();
    };
    $.Arwend.ReInitialize = function () {
        $(document).ready(function () {
            $('.only-numeric').unbind("keypress");
            $('.only-alphabet').unbind("keypress");
            $('.only-alphaNumeric').unbind("keypress");
            $('.only-decimal').unbind("keypress");
            $('.to-upper').unbind("keyup");
            $('.phone-number').unbind("keypress");
            $("input").unbind("blur");

            $('.only-numeric').bind('keypress', function (event) { return $.Arwend.IsValid(event, $.Arwend.Patterns.Numeric.Regex); });
            $('.only-alphabet').bind('keypress', function (event) { return $.Arwend.IsValid(event, $.Arwend.Patterns.Alphabet.Regex); });
            $('.only-alphaNumeric').bind('keypress', function (event) { return $.Arwend.IsValid(event, $.Arwend.Patterns.AlphaNumeric.Regex); });
            $('.only-decimal').bind('keypress', function (event) { return $.Arwend.IsValidDecimal(event); });
            $('.to-upper').bind('keyup', function () { $.Arwend.ToUpper(this); });
            $('.phone-number').bind('keypress', function (event) { return !$.Arwend.GetChar(event).StartsWith('0'); });
            $("input").blur(function () { if (this.value != '') $(this).removeClass('form-error'); });
        });
    }
    $.Arwend.Patterns = {
        AlphaNumeric: { Regex: /^[a-zA-Z0-9şŞıİçÇöÖüÜĞğ]+$/ },
        Alphabet: { Regex: /[^a-zA-ZşŞıİçÇöÖüÜĞğ]/ },
        Numeric: { Regex: /^-{0,1}\d*\.{0,1}\d+$/ },
        Email: { Regex: /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/ }
    };
    $.Arwend.Ajax = {
        IsCompleted: true,
        Parameters: null,
        Request: null,
        GetResponse: function (callBackFunction, cache, async) {
            if (this.IsCompleted) {
                var action = this.Parameters.Action;
                this.Request = $.ajax({
                    type: "POST",
                    beforeSend: function (XMLHttpRequest) {
                        XMLHttpRequest.setRequestHeader('Content-type', 'application/x-www-form-urlencoded; charset=utf-8');
                    },
                    dataType: "json",
                    contentType: "application/x-www-form-urlencoded; charset=utf-8",
                    url: "/ajax/" + action + "?rnd=" + $.Arwend.GetMiliseconds() + $.Arwend.Randomize(),
                    data: this.Parameters,
                    cache: (cache == null || cache == undefined ? false : cache),
                    async: (async == null || async == undefined ? false : async),
                    success: function (response) {
                        this.IsCompleted = true;
                        if (response != 'NOT_AUTHORIZED') {
                            if (response != null && response != undefined) {
                                if (callBackFunction != null && callBackFunction != undefined)
                                    callBackFunction(response);
                            }
                            else {
                                alert('Hata','İşlem sırasında bir hata oluştu! Lütfen tekrar deneyiniz.',Error);
                                if ($(".loading").exists()) $(".loading").remove();
                            }
                        }
                        else {
                            var Message = response.split('###')[1];
                            if (Message == null || Message == undefined || Message == '') Message = 'Oturumunuz sona erdi. Lütfen tekrar giriş yapınız.';
                            alert('Bilgi',Message,'');
                        }
                    },
                    error: function (e) { alert('Hata','İşlem sırasında bir hata oluştu! Lütfen tekrar deneyiniz.',Error); this.IsCompleted = true; }
                });
            }
        },
        AbortCurrentRequest: function () {
            if (this.Request != undefined && this.Request != null && this.Request.readystate != 4)
                this.Request.abort();
        }
    }
    $.Arwend.FocusToElement = function (controlID) {
        if ($('#' + controlID).exist()) $('#' + controlID).focus();
    }
    $.Arwend.IsNumericKey = function (event) {
        var keycode = event.charCode ? event.charCode : event.keyCode ? event.keyCode : event.which;
        if (!event.shiftKey) {
            if ($.Arwend.Ignored(keycode) || (keycode > 47 && keycode < 58)) {
                return true;
            }
        }
        return false;
    }
    $.Arwend.IsDecimalKey = function (event) {
        var keycode = event.charCode ? event.charCode : event.keyCode ? event.keyCode : event.which;
        var NewValue = this.value + String.fromCharCode(keycode);
        var Result = true;
        Result = !NewValue.match(/[^0-9.]/g);
        var i = 0;
        NewValue = NewValue.replace(/\./g, function (text) {
            if (i == 0) { i++; return text; }
            else { Result = false; }
        });
        if (NewValue.indexOf('.') == 0) { Result = false; }
        return Result;
    }
    $.Arwend.IsValid = function (event, regexCode) {
        var keycode = event.charCode ? event.charCode : event.keyCode ? event.keyCode : event.which;
        return !String.fromCharCode(keycode).match(regexCode);
    }
    $.Arwend.IsSelectBoxSelected = function (controlID) {
        var control = document.getElementById(controlID);
        return !(!$(control).exist() || $(control).val() == "-1" || $(control).val() == "Seçiniz" || $(control).val() == "");
    }
    $.Arwend.GetChar = function (event) {
        var keycode = event.charCode ? event.charCode : event.keyCode ? event.keyCode : event.which;
        return String.fromCharCode(keycode);
    }
    $.Arwend.Ignored = function (keycode) {
        return !(keycode == 8 || keycode == 32 || keycode == 9 || keycode == 13);
    }
    $.Arwend.ToUpper = function (control) {
        var start = control.selectionStart, end = control.selectionEnd;
        control.value = control.value.toTrkUppercase();
        control.setSelectionRange(start, end);
    }
    $.Arwend.FocusToElement = function (controlID) {
        if ($('#' + controlID).exist()) $('#' + controlID).focus();
    }
    $.Arwend.Randomize = function () {
        return ((Math.floor(Math.random() * 10000)) + 100);
    }
    $.Arwend.GetMiliseconds = function () {
        var date = new Date();
        return date.getTime();
    }
    $.Arwend.GetEnumByType = function (enumTypes, type) {
        for (var i in enumTypes) if (enumTypes[i].Type == type) return enumTypes[i];
        return null;
    }
    $.Arwend.GetEnumByID = function (enumTypes, id) {
        for (var i in enumTypes) if (enumTypes[i].ID == id) return enumTypes[i];
        return null;
    }
    $.Arwend.GetEnumByName = function (enumTypes, name) {
        for (var i in enumTypes) if (enumTypes[i].Name == Name) return enumTypes[i];
        return null;
    }
    $.Arwend.ScrollToElement = function (elementId) {
        if ($('#' + elementId).length > 0) {
            $('html, body').animate({
                scrollTop: $('#' + elementId).offset().top - 80
            }, 10);
        }
    }
    $.Arwend.FullScreen = function () {
        if (!document.fullscreenElement &&    // alternative standard method
            !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) {  // current working methods
            if (document.documentElement.requestFullscreen) {
                document.documentElement.requestFullscreen();
            } else if (document.documentElement.msRequestFullscreen) {
                document.documentElement.msRequestFullscreen();
            } else if (document.documentElement.mozRequestFullScreen) {
                document.documentElement.mozRequestFullScreen();
            } else if (document.documentElement.webkitRequestFullscreen) {
                document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
            }
        } else {
            if (document.exitFullscreen) {
                document.exitFullscreen();
            } else if (document.msExitFullscreen) {
                document.msExitFullscreen();
            } else if (document.mozCancelFullScreen) {
                document.mozCancelFullScreen();
            } else if (document.webkitExitFullscreen) {
                document.webkitExitFullscreen();
            }
        }
    }
    $.fn.SerializeFormJSON = function (action, parameters) {
        var o = {};
        var a = $(this).serializeArray();
        a.push({ name: 'Action', value: action });
        a.push({ name: 'Protocol', value: window.location.protocol });
        if (parameters != null)
            a = $.merge($.merge([], parameters), a);
        $.each(a, function (index, value) {
            o[value.name] = o[value.name] ? o[value.name] || value.value : value.value;
        });
        return o;
    };

    $.Arwend.Initialize();
}(jQuery));

/* Prototypes */
Date.prototype.monthsOfYear = {
    January: 1, February: 2, March: 3, April: 4, May: 5, June: 6, July: 7, August: 8, September: 9, October: 10, November: 11, December: 12,
    GetName: function (index) {
        switch (index) {
            case 1: return "Ocak"; break;
            case 2: return "Şubat"; break;
            case 3: return "Mart"; break;
            case 4: return "Nisan"; break;
            case 5: return "Mayıs"; break;
            case 6: return "Haziran"; break;
            case 7: return "Temmuz"; break;
            case 8: return "Ağustos"; break;
            case 9: return "Eylül"; break;
            case 10: return "Ekim"; break;
            case 11: return "Kasım"; break;
            case 12: return "Aralık"; break;
        }
    }
};
Date.prototype.daysOfWeek = {
    Sunday: 0, Monday: 1, Tuesday: 2, Wednesday: 3, Thursday: 4, Friday: 5, Saturday: 6,
    GetName: function (index) {
        switch (index) {
            case 0: return "Pazar"; break;
            case 1: return "Pazartesi"; break;
            case 2: return "Salı"; break;
            case 3: return "Çarşamba"; break;
            case 4: return "Perşembe"; break;
            case 5: return "Cuma"; break;
            case 6: return "Cumartesi"; break;
        }
    }
};
Date.isLeapYear = function (year) {
    return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
};
Date.getDaysInMonth = function (year, month) {
    return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
};
Date.prototype.isLeapYear = function () {
    var y = this.getFullYear();
    return (((y % 4 === 0) && (y % 100 !== 0)) || (y % 400 === 0));
};
Date.prototype.getDaysInMonth = function () {
    return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
};
Date.prototype.addDays = function (value) {
    var n = this.getDate();
    this.setDate(n + value);
    return this;
};
Date.prototype.addMonths = function (value) {
    var n = this.getDate();
    this.setDate(1);
    this.setMonth(this.getMonth() + value);
    this.setDate(Math.min(n, this.getDaysInMonth()));
    return this;
};
Date.prototype.addYears = function (value) {
    this.setFullYear(this.getFullYear() + value);
    return this;
};
Date.prototype.getMonthName = function () {
    return this.monthsOfYear.GetName(this.getMonth() + 1);
};
Date.prototype.getDayName = function () {
    return this.daysOfWeek.GetName(this.getDay());
};
Date.prototype.getFirstDayOfMonth = function () {
    return new Date(this.getFullYear(), this.getMonth(), 1).getDay();
};
Date.prototype.getCalendar = function () {
    return GetMonthlyActivities(this.getFullYear(), this.getMonth() + 1);
};
Date.prototype.getDailyActivities = function () {
    return GetDailyActivities();
};
Date.prototype.convertToDateTime = function () {
    return this.getDate().toString() + '.' + (this.getMonth() + 1).toString() + '.' + this.getFullYear().toString();
};
String.prototype.isValidIdentificationNumber = function () {
    var Result = true;
    if (this.length != 11 || !this.IsNumeric())
        Result = false;
    else {
        var IdentificationNumber = parseInt(this);
        var d = new Array(10);
        var tmp1 = null;
        var tmp = null;
        var odd_sum = null;
        var even_sum = null;
        var total = null;
        var chkDigit1 = null;
        var chkDigit2 = null;
        if (IdentificationNumber > 0) {
            tmp = Math.floor(IdentificationNumber / 100);
            tmp1 = Math.floor(IdentificationNumber / 100);
            for (var i = 9; i > 0; i--) {
                d[i] = tmp1 % 10;
                tmp1 = Math.floor(tmp1 / 10);
            }
            odd_sum = d[9] + d[7] + d[5] + d[3] + d[1];
            even_sum = d[8] + d[6] + d[4] + d[2];
            total = odd_sum * 7 - even_sum;
            chkDigit1 = total % 10;
            odd_sum = chkDigit1 + d[8] + d[6] + d[4] + d[2];
            even_sum = d[9] + d[7] + d[5] + d[3] + d[1];
            total = odd_sum + even_sum;
            chkDigit2 = total % 10;
            tmp = tmp * 100 + chkDigit1 * 10 + chkDigit2;
            if (tmp != IdentificationNumber) Result = false;
        }
    }
    return Result;
}
String.prototype.toTrkUppercase = function () {
    var str = [];
    for (var i = 0; i < this.length; i++) {
        var ch = this.charCodeAt(i);
        var c = this.charAt(i);
        if (ch == 105) str.push('İ');
        else if (ch == 305) str.push('I');
        else if (ch == 287) str.push('Ğ');
        else if (ch == 252) str.push('Ü');
        else if (ch == 351) str.push('Ş');
        else if (ch == 246) str.push('Ö');
        else if (ch == 231) str.push('Ç');
        else if (ch >= 97 && ch <= 122)
            str.push(c.toUpperCase());
        else
            str.push(c);
    }
    return str.join('');
}
String.prototype.isNullOrEmpty = function () {
    return this == null || this == undefined || this.trim() == "" || this.length === 0;
}
String.prototype.startsWith = function (value) {
    return this.charAt(0) == value;
}
String.prototype.isAtRange = function (min, max) {
    try {
        if (this.isNumeric())
            return this >= min && this <= max;
        return false;
    }
    catch (e) { return false; }
}
String.prototype.hasValidLength = function (min, max) {
    return this.length >= min && this.length <= max;
}
String.prototype.isNumeric = function () {
    try { return $.Arwend.Patterns.Numeric.Regex.test(this); }
    catch (e) { return false; }
}
String.prototype.isValidEmail = function () {
    try { return $.Arwend.Patterns.Email.Regex.test(this); }
    catch (e) { return false; }
}
String.prototype.isEqual = function (value) {
    return this == Value;
}
if (!String.prototype.trim) {
    String.prototype.trim = function () {
        return $.trim(this);
    }
}

function LoadStyle(url) {
    var head = document.getElementsByTagName('head')[0] || (_QUIRKS ? document.body : document.documentElement),
		link = document.createElement('link'),
		absoluteUrl = _chopQuery(_formatUrl(url, 'absolute'));
    var links = K('link[rel="stylesheet"]', head);
    for (var i = 0, len = links.length; i < len; i++) {
        if (_chopQuery(_formatUrl(links[i].href, 'absolute')) === absoluteUrl) {
            return;
        }
    }
    head.appendChild(link);
    link.href = url;
    link.rel = 'stylesheet';
}

function LoadScript(url, fn) {
    var head = document.getElementsByTagName('head')[0] || (_QUIRKS ? document.body : document.documentElement),
		script = document.createElement('script');
    head.appendChild(script);
    script.src = url;
    script.charset = 'utf-8';
    script.onload = script.onreadystatechange = function () {
        if (!this.readyState || this.readyState === 'loaded') {
            if (fn) {
                fn();
            }
            script.onload = script.onreadystatechange = null;
            head.removeChild(script);
        }
    };
}