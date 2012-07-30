package com.motion{
	import flash.display.MovieClip;
	import flash.events.TimerEvent;
	import flash.utils.Timer;

	/**
	 * 定时转15度  做跑马灯效果
	 * */
	public class PaoMaDengRotation{
		private var timer:Timer ;
		private var mc:MovieClip
		
		private var timerStop:Timer ;
		
		private var cnt:int = 1 ;
		private var delay:Number = 200;
		/**
		 * isClockwise : 是否是顺时针
		 * */
		public function PaoMaDengRotation(mc:MovieClip,isClockwise:Boolean=true){
			this.mc = mc ;
			
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
				delay = delay/2 ;
				timer = new Timer(delay,0); 
				timer.addEventListener("timer", timerHandler);
				timer.start();
			}
			mc.rotation += 15 ;
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
			if(mc.rotation % 360 == 0){
				timer.stop() ;
				timer = null ;
				return ;
			}
			mc.rotation += 15 ;
		}
	}
}