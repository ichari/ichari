package com.motion{
	import flash.display.MovieClip;
	import flash.events.TimerEvent;
	import flash.utils.Timer;

	/**
	 * 
	 * */
	public class RotationTween{
		private var timer:Timer ;
		private var mc:MovieClip
		
		private var timerStop:Timer ;
		
		private var cnt:int = 1 ;
		private var delay:Number = 200;
		private var angle:int ;
		
		private var v = 5 ;
		/**
		 * isClockwise : 是否是顺时针
		 * angle : 是整数 30 60 90 ...
		 * */
		public function RotationTween(mc:MovieClip,isClockwise:Boolean=true,angle:Number=0){
			this.mc = mc ;
			this.angle = angle ;
			if(!isClockwise){
				this.v = -5 ;
			}
			
			timer = new Timer(delay,0); 
			timerStop = new Timer(8*1000,1);
			
			timer.addEventListener("timer", timerHandler);
			timerStop.addEventListener("timer", timerStopHandler);
			timer.start();
			timerStop.start();
			
		}
		
		/**
		 * 定时器开始
		 * */
		public function timerHandler(event:TimerEvent):void {
			if(cnt % 3 == 0 ){
				timer.stop() ;
				timer = null ;
				delay = delay/4 ;
				timer = new Timer(delay,0); 
				timer.addEventListener("timer", timerHandler);
				timer.start();
			}
			mc.rotation += v ;
			cnt ++ ;
		}
		
		/**
		 * 定时器结束
		 * */
		public function timerStopHandler(event:TimerEvent):void {
			timer.stop() ;
			timer = null ;
			timerStop = null ;
			
			timer = new Timer(20,0); 
			timer.addEventListener("timer", lastTimerHandler);
			timer.start();
		}
		
		
		public function lastTimerHandler(event:TimerEvent):void {
			trace(mc.rotation + "==================="+angle);
			if(mc.rotation % angle == 0){
				timer.stop() ;
				timer = null ;
				return ;
			}
			mc.rotation += v ;
		}
	}
}