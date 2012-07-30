var CountDown = {
    timerID: null,
    timerRunning: false,
    IsuseEndTime: null, //期号截止时间
    times: new Array(),
    serverTime: null,
    ctpl: '还剩 <span class="last_time"> {0}天{1}小时</span>',
    ctpl2: '还剩 <span class="last_time">{1}小时{2}分钟{3}秒</span>',
    timebar: '', //显示板
    fps: 1000,

    showtime: function() {
        var now;
        this.now += this.fps;
        now = this.now;

        var To = this.getDate(this.IsuseEndTime).getTime() - now;

        this.times = new Array();
        this.times[0] = Math.floor(To / (1000 * 60 * 60 * 24));
        this.times[1] = Math.floor(To / (1000 * 60 * 60)) % 24;
        this.times[2] = Math.floor(To / (1000 * 60)) % 60;
        this.times[3] = Math.floor(To / 1000) % 60;

        if (this.times[3] == 1) {
            var htmlobj = $.ajax({ url: "/ajax/getServerTime.ashx?rnd=" + Math.random(), async: false });
            $('#HidServerTime').val(htmlobj.responseText);
            this.serverTime = htmlobj.responseText;
        }

        var tpl = this.times[0] > 0 ? this.ctpl : this.ctpl2;

        if (To < 0) {
            this.timebar.html('<span class="red">已截止</span>');
        }
        else {
            this.timebar.html(this.format(tpl, this.times).replace(/\b\d\b/g, '0$&'))
        }

        this.timerRunning = true;
    },

    stopclock: function() {
        if (this.timerRunning)
            clearInterval(this.timerID);
        this.timerRunning = false;
    },

    startclock: function() {
        this.stopclock();
        var Y = this;
        this.now = this.getDate(this.serverTime).getTime();
        this.timerID = setInterval(function() {
            Y.showtime();
        }, 1000);

        Y.showtime();
    },

    format: function(source, params) {
        if (arguments.length == 1)
            return function() {
                var args = $.makeArray(arguments);
                args.unshift(source);
                return $.format.apply(this, args);
            };
        if (arguments.length > 2 && params.constructor != Array) {
            params = $.makeArray(arguments).slice(1);
        }
        if (params.constructor != Array) {
            params = [params];
        }
        $.each(params, function(i, n) {
            source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
        });
        return source;
    },

    getDate: function(date, def) {
        var date = new Date(date ? date.replace(/-/g, '/') : date);
        return isNaN(date) ? def : date;
    }
}