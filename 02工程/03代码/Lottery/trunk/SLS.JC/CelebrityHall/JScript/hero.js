(function(A) {
    function _ROLL(obj) {
        this.ele = document.getElementById(obj);
        this.interval = false;
        this.currentNode = 0;
        this.passNode = 0;
        this.speed = 100;
        this.childs = _childs(this.ele);
        this.childHeight = parseInt(_style(this.childs[0])['height']);
        addEvent(this.ele, 'mouseover', function() {
            window._loveYR.pause();
        });
        addEvent(this.ele, 'mouseout', function() {
            window._loveYR.start(_loveYR.speed);
        });
    }
    
    function _style(obj) {
        if (!obj) {
            return 0;
        }
        return obj.currentStyle || document.defaultView.getComputedStyle(obj, null);
    }
    
    function _childs(obj) {
        var childs = [];
        for (var i = 0; i < obj.childNodes.length; i++) {
            var _this = obj.childNodes[i];
            if (_this.nodeType === 1) {
                childs.push(_this);
            }
        }
        return childs;
    }
    
    function addEvent(elem, evt, func) {
        if (-[1, ]) {
            elem.addEventListener(evt, func, false);
        } else {
            elem.attachEvent('on' + evt, func);
        };
    }
    
    function innerest(elem) {
        var c = elem;
        while (c.childNodes.item(0).nodeType == 1) {
            c = c.childNodes.item(0);
        }
        return c;
    }
    
    _ROLL.prototype = {
        start: function(s) {
            var _this = this;
            _this.speed = s || 100;
            _this.interval = setInterval(function() {
                _this.ele.scrollTop += 1;
                _this.passNode++;
                if (_this.passNode % _this.childHeight == 0) {
                    var o = _this.childs[_this.currentNode] || _this.childs[0];
                    _this.currentNode < (_this.childs.length - 1) ? _this.currentNode++ : _this.currentNode = 0;
                    _this.passNode = 0;
                    _this.ele.scrollTop = 0;
                    _this.ele.appendChild(o);
                }
            }, _this.speed);
        },
        pause: function() {
            var _this = this;
            clearInterval(_this.interval);
        }
    }
    A.marqueen = function(obj) { A._loveYR = new _ROLL(obj); return A._loveYR; }
})(window);
marqueen('roll').start(100/*速度默认100*/);
