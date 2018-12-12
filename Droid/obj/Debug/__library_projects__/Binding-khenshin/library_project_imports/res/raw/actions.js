
var Khipu = {}

Khipu.actions = {

	select : function(target, idx) {
		target.selectedIndex = idx;

		var evt = document.createEvent('HTMLEvents');

		evt.initEvent('change', false, true);
		target.dispatchEvent(evt);
	},

	simulateKeyEvent : function(target, type, bubbles, cancelable, view,
			ctrlKey, altKey, shiftKey, metaKey, keyCode, charCode) {
		bubbles = typeof bubbles !== 'undefined' ? bubbles : true;
		cancelable = typeof cancelable !== 'undefined' ? cancelable : true;
		view = typeof view !== 'undefined' ? view : window;
		ctrlKey = typeof ctrlKey !== 'undefined' ? ctrlKey : false;
		altKey = typeof altKey !== 'undefined' ? altKey : false;
		shiftKey = typeof shiftKey !== 'undefined' ? shiftKey : false;
		metaKey = typeof metaKey !== 'undefined' ? metaKey : false;
		keyCode = typeof keyCode !== 'undefined' ? keyCode : 0;
		charCode = typeof charCode !== 'undefined' ? charCode : 0;

		var customEvent = null;

		try {

			customEvent = document.createEvent("KeyEvents");

			customEvent.initKeyEvent(type, bubbles, cancelable, view, ctrlKey,
					altKey, shiftKey, metaKey, keyCode, charCode);

		} catch (ex) {
			try {
				customEvent = document.createEvent("Events");
			} catch (uierror) {
				// the above failed, so create a UIEvent for Safari 2.x
				customEvent = document.createEvent("UIEvents");
			} finally {
				customEvent.initEvent(type, bubbles, cancelable);
				customEvent.view = view;
				customEvent.altKey = altKey;
				customEvent.ctrlKey = ctrlKey;
				customEvent.shiftKey = shiftKey;
				customEvent.metaKey = metaKey;
				customEvent.keyCode = keyCode;
				customEvent.charCode = charCode;
			}
		}

		target.dispatchEvent(customEvent);
	},
	simulateMouseEvent : function(target, type, bubbles, cancelable, view,
			detail, screenX, screenY, clientX, clientY, ctrlKey, altKey,
			shiftKey, metaKey, button, relatedTarget, doc) {
		relatedTarget = relatedTarget || null;
		bubbles = typeof bubbles !== 'undefined' ? bubbles : true;
		cancelable = typeof cancelable !== 'undefined' ? cancelable
				: (type !== "mousemove");
		view = typeof view !== 'undefined' ? view : window;
		detail = typeof detail !== 'undefined' ? detail : 1;
		screenX = typeof screenX !== 'undefined' ? screenX : 0;
		screenY = typeof screenY !== 'undefined' ? screenY : 0;
		clientX = typeof clientX !== 'undefined' ? clientX : 0;
		clientY = typeof clientY !== 'undefined' ? clientY : 0;
		ctrlKey = typeof ctrlKey !== 'undefined' ? ctrlKey : false;
		altKey = typeof altKey !== 'undefined' ? altKey : false;
		shiftKey = typeof shiftKey !== 'undefined' ? shiftKey : false;
		metaKey = typeof metaKey !== 'undefined' ? metaKey : false;
		button = typeof button !== 'undefined' ? button : 0;

		doc = typeof doc !== 'undefined' ? doc : document;
		var customEvent = doc.createEvent("MouseEvents");

		customEvent.initMouseEvent(type, bubbles, cancelable, view, detail,
				screenX, screenY, clientX, clientY, ctrlKey, altKey, shiftKey,
				metaKey, button, relatedTarget);
		if (relatedTarget && !customEvent.relatedTarget) {
			if (type === "mouseout") {
				customEvent.toElement = relatedTarget;
			} else if (type === "mouseover") {
				customEvent.fromElement = relatedTarget;
			}
		}
		target.dispatchEvent(customEvent);
	},

	fireMouseEvent : function(target, type, options) {
		options = options || {};
		this.simulateMouseEvent(target, type, options.bubbles,
				options.cancelable, options.view, options.detail,
				options.screenX, options.screenY, options.clientX,
				options.clientY, options.ctrlKey, options.altKey,
				options.shiftKey, options.metaKey, options.button,
				options.relatedTarget, options.doc);
	},
	iframeClick : function(target, iframeSelector, options) {
		options = options || {};
		options.doc = document.querySelector(iframeSelector).contentWindow.document;
		options.view = document.querySelector(iframeSelector).contentWindow;
		this.fireMouseEvent(target, "click", options);
	},
	click : function(target, options) {
		this.fireMouseEvent(target, "click", options);
	},
	dblclick : function(target, options) {
		this.fireMouseEvent(target, "dblclick", options);
	},
	mousedown : function(target, options) {
		this.fireMouseEvent(target, "mousedown", options);
	},
	mousemove : function(target, options) {
		this.fireMouseEvent(target, "mousemove", options);
	},
	mouseout : function(target, options) {
		this.fireMouseEvent(target, "mouseout", options);
	},
	mouseover : function(target, options) {
		this.fireMouseEvent(target, "mouseover", options);
	},
	mouseup : function(target, options) {
		this.fireMouseEvent(target, "mouseup", options);
	},
	fireKeyEvent : function(type, target, options) {
		options = options || {};
		this.simulateKeyEvent(target, type, options.bubbles,
				options.cancelable, options.view, options.ctrlKey,
				options.altKey, options.shiftKey, options.metaKey,
				options.keyCode, options.charCode);
	},
	keydown : function(target, options) {
		this.fireKeyEvent("keydown", target, options);
	},
	keypress : function(target, options) {
		this.fireKeyEvent("keypress", target, options);
	},
	keyup : function(target, options) {
		this.fireKeyEvent("keyup", target, options);
	}
};

Khipu.findIframeClick = function(selector, iframeSelector) {
	var target = document.querySelector(iframeSelector).contentWindow.document
			.querySelector(selector);
	if (target) {
		Khipu.actions.iframeClick(target, iframeSelector);
	}
}

Khipu.findClick = function(selector, frameName) {
	var target = typeof frameName !== 'undefined' ? frames[frameName].document
			.querySelector(selector) : document.querySelector(selector);
	if (target) {
		Khipu.actions.click(target);
	}
}

Khipu.find = function(selector, frameName) {
	if (typeof frameName !== 'undefined') {
		return frames[frameName].document.querySelector(selector);
	}
	return document.querySelector(selector);
}

Khipu.iframeFind = function(selector, iframeSelector) {
	var iframe = document.querySelector(iframeSelector);
	if (iframe !== null) {
		return iframe.contentWindow.document.querySelector(selector);
	}
	return null
}

Khipu.iframeRemove = function(selector, iframeSelector) {
	var node = Khipu.iframeFind(selector, iframeSelector);
	if (node) {
		node.parentNode.removeChild(node);
	}
}

Khipu.iframeFindAll = function(selector, iframeSelector) {
	var iframe = document.querySelector(iframeSelector);
	if (iframe !== null) {
		return iframe.contentWindow.document.querySelectorAll(selector);
	}
	return null
}

Khipu.findAll = function(selector, frameName) {
	if (typeof frameName !== 'undefined') {
		return frames[frameName].document.querySelectorAll(selector);
	}
	return document.querySelectorAll(selector);
}

Khipu.setInputValue = function(selector, value, frameName) {
	var element;
	if (typeof frameName !== 'undefined') {
		element = frames[frameName].document.querySelector(selector);
	} else {
		element = this.find(selector);
	}
	if (element) {
		element.focus();
		element.value = value;
		var evt = document.createEvent('HTMLEvents');
		evt.initEvent('change', false, true);
		element.dispatchEvent(evt);
	}
}

Khipu.setIframeInputValue = function(selector, iframeSelector, value) {
	var element = this.iframeFind(selector, iframeSelector);
	if (element) {
		element.focus();
		element.value = value;
		var evt = document.createEvent('HTMLEvents');
		evt.initEvent('change', false, true);
		element.dispatchEvent(evt);
	}
}

Khipu.doTests = function() {
	if (!KhipuAutomaton.doTests()) {
		var to = setTimeout(Khipu.doTests, 500);
		return;
	}
}

Khipu.test = function(exp, selector, frameName) {
	var element = this.find(selector, frameName);
	if (element) {
		return exp.test(element.innerHTML);
	}
}

Khipu.iframeTest = function(exp, selector, iframeSelector) {
	var element = this.iframeFind(selector, iframeSelector);
	if (element) {
		return exp.test(element.innerHTML);
	}
}

Khipu.testInnerHTML = function(val, selector, frameName) {
	var element = this.find(selector, frameName);

	if (element) {
		return element.innerHTML == val;
	}
}

Khipu.selectByValue = function(selector, value, frameName) {
	var idx = -1;
	var options = Khipu.findAll(selector + ' option', frameName);

	for ( var i = 0; i < options.length; i++) {
		if (options[i].value == value) {
			idx = i;
		}
	}

	if (idx > -1) {
		Khipu.actions.select(Khipu.find(selector, frameName), idx);
	}
}

Khipu.selectByInnerHTML = function(selector, value, frameName) {
	var idx = -1;
	var options = Khipu.findAll(selector + ' option', frameName);
	for ( var i = 0; i < options.length; i++) {
		if (options[i].innerHTML.match(value) !== null) {
			idx = i;
		}
	}

	if (idx > -1) {
		Khipu.actions.select(Khipu.find(selector, frameName), idx);
	}
}

Khipu.findIndexByInnerHTML = function(selector, value, frameName) {

	var options = Khipu.findAll(selector, frameName);
	for ( var i = 0; i < options.length; i++) {
		if (options[i].innerHTML.match(value) !== null) {
			return i;
		}
	}
	return -1;
}

Khipu.findSelectByInnerHTML = function(selector, value, frameName) {
	return Khipu.findIndexByInnerHTML(selector + ' option', value, frameName);
}

Khipu.findIndexByValue = function(selector, value, frameName) {
	var options = Khipu.findAll(selector, frameName);

	for ( var i = 0; i < options.length; i++) {
		if (options[i].value == value) {
			return i;
		}
	}
	return -1;
}

Khipu.findSelectByValue = function(selector, value, frameName) {
	Khipu.findIndexByValue(selector + ' option', value, frameName);
}

Khipu.findByInnerHTML = function(selector, value, frameName) {

	var options = Khipu.findAll(selector, frameName);

	for ( var i = 0; i < options.length; i++) {
		if (options[i].innerHTML.match(value) !== null) {
			return options[i];
		}
	}
	return null;
}

Khipu.findByValue = function(selector, value, frameName) {

	var options = Khipu.findAll(selector, frameName);

	for ( var i = 0; i < options.length; i++) {
		if (options[i].value == value) {
			return options[i];
		}
	}
	return null;
}

Khipu.lastAlert = [];

Khipu.testAlert = function(exp) {
	if (exp.test(Khipu.lastAlert[0])) {
		Khipu.lastAlert = null;
		return true;
	}
}

Khipu.getAlertMessage = function() {
	if (Khipu.lastAlert && Khipu.lastAlert.length > 0) {
		return Khipu.lastAlert[0];
	}
	return "";
}

Khipu.sleep = function(milliSeconds) {
	var startTime = new Date().getTime();
	while (new Date().getTime() < startTime + milliSeconds)
		;
}

Khipu.tableValueByInnerHTML = function(selector, colRec, regex, colValue,
		frameName) {
	var tabla = Khipu.findAll(selector + ' tr', frameName);
	for ( var i = 0; i < tabla.length; i++) {
		if (regex.test(tabla[i].querySelectorAll('td')[colRec].innerHTML)) {
			return tabla[i].querySelectorAll('td')[colValue].innerHTML;
		}
	}
}

Khipu.getJavascriptVariables = function() {
	var khipuVariables = {};
	for (v in window) {
		if (window[v] && window[v].toString().indexOf('native code') < 0) {
			if (typeof window[v] === 'number'
					|| (typeof window[v]) === 'string') {
				khipuVariables[v] = window[v];
			} else {
				khipuVariables[v] = typeof window[v];
			}
		}
	}
	return khipuVariables;
}

Khipu.getAlertMessage = function() {
	return Khipu.lastAlert.length > 0 ? Khipu.lastAlert[0] : null;
}

Khipu.getHtml = function() {
	var xmlSerializer = new XMLSerializer();

	var html = {
		'main-frame' : {
			'url' : window.location.href,
			'source' : xmlSerializer.serializeToString(document)
		}
	};

	if (typeof frames !== 'undefined') {
		Khipu.getFrames(frames, html);
	}

	return html;
}

Khipu.getFrames = function(frameset, map) {
	var xmlSerializer = new XMLSerializer();

	for ( var f = 0; f < frameset.length; f++) {
		var frame = frameset[f];
		var url, source, frameName;
		try {
			frameName = typeof frame.name !== 'undefined'
				&& frame.name.length > 0 ? frame.name : 'frame-'
				+ (Object.keys(map).length + 1);
			url = frame.location.href;
			source = xmlSerializer.serializeToString(frame.document)
		} catch (e) {
			console.error("Couldn't fetch frame attributes: " + e);
			url = 'error';
			source = 'error';
			frameName = 'error';
		}

		map[frameName] = {
			'url' : url,
			'source' : source
		};

		if (typeof frame.frames !== 'undefined') {
			Khipu.getFrames(frame.frames, map);
		}
	}
}



Khipu.getCookieValue = function(cookieName) {
	var name = cookieName + "=";
	var ca = document.cookie.split(';');
	for(var i=0; i<ca.length; i++) {
		var c = ca[i];
		while (c.charAt(0) == ' ') c = c.substring(1);
		if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
	}
	return "";
}

Khipu.storeCookie = function(cookieName) {
	KhipuAutomaton.storeCookie(cookieName, document.location.protocol, document.location.hostname, Khipu.getCookieValue(cookieName));
}


if (!('remove' in Element.prototype)) {
	Element.prototype.remove = function() {
		if (this.parentNode) {
			this.parentNode.removeChild(this);
		}
	};
}