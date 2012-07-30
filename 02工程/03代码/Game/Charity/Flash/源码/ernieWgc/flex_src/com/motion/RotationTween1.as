package com.motion{
	import com.motion.GTween;
	import com.motion.GTweenTimeline;
	
	import fl.motion.easing.*;
	
	import flash.display.MovieClip;
	import flash.events.TimerEvent;
	import flash.utils.Timer;

	public class RotationTween1{
		
		public var timer:Timer ;
		public var rotationCnt:Number = 0 ;
		public var v:int = 1 ;
		public var g:Number ; 
		public var a:Number ;
		public var isJiaSu:Boolean = true ;
		
		public var mc:MovieClip = null ;
		public var angle:Number ;
		
		
		public function RotationTween1(mc:MovieClip,angle:Number,g:Number=1){
			
			this.mc = mc ; 
			this.angle = angle ;
			this.g = g ;
			a = g ;
			timer = new Timer(100,0);
			timer.addEventListener("timer", timerHandler);
			timer.start();
		}
		
		public function timerHandler(event:TimerEvent):void {
			
			v += a ;
			rotationCnt += v  ;
			if(rotationCnt % 60 == 0 && isJiaSu){
				a += g ;
			}
			if(!isJiaSu){
				if(Math.abs(v) <= 2){
					timer.stop();
					new GTween(mc,1,{repeat:0,rotation:angle%360},{ease:Quartic.easeInOut,reflect:false});
				}
				a -= g ;
			}
			if(Math.abs(v) > 40 ){
				isJiaSu = false ;
			}
			mc.rotation = rotationCnt % 360 ;
			
		}
		
		
		

	}
}