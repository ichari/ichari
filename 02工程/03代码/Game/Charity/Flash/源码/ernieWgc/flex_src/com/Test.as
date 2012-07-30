package com{
	import com.Gou;
	import com.motion.*;
	
	import fl.motion.easing.*;
	
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.MovieClip;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.geom.Point;

	public class Test extends MovieClip{
		public var gouData:BitmapData ;
		public var gou:Bitmap ;
		public var lvdianData:BitmapData ;
		public var lvdian:Bitmap ;
		
		public var gouMc:MovieClip ;
		public var lvDianMc:MovieClip ;
		
		public function Test(){
			/*gouData = new BitmapData(30,55,true,0x00000000);
			gouData.draw(new Gou());
			gou = new Bitmap(gouData,"auto",true);
			gou.x = 327 ;
			gou.y = 41 ;
			this.addChild(gou);
			
			lvdianData = new BitmapData(560,560,true,0x00000000);
			lvdianData.draw(new Lvdian());
			lvdian = new Bitmap(lvdianData,"auto",true);
			lvdian.x = 63.3 ;
			lvdian.y = 81.3 ;
			this.addChild(lvdian);*/
			
			gouMc = new Gou() ;
			this.addChild(gouMc);
			gouMc.x = 327 ;
			gouMc.y = 41 ;
			
			lvDianMc = new Lvdian();
			this.addChild(lvDianMc);
			lvDianMc.x = 341.3 ;
			lvDianMc.y = 363.3 ;
			
			var tween6:GTween = new GTween(lvDianMc,10,{rotation:360*5-180},{repeat:0,ease:Quartic.easeOut,reflect:false,autoRotation:false});
			this.addEventListener(Event.ENTER_FRAME,enterFrame);
			
		}
		
		function enterFrame(e:Event):void{
			//if (gouData.hitTest(new Point(gou.x,gou.y),255,lvdian,new Point(lvdian.x,lvdian.y),255)) {
			
			if(lvDianMc.hitTestPoint(340,90,false)){
				gouMc.rotation += 30 ;
			}else{
				//gouMc.rotation -=1 ;
			}
		}
		
		function zhuanHudu(angle:Number):Number{
			return angle*Math.PI/180 ;
		}
	}
}