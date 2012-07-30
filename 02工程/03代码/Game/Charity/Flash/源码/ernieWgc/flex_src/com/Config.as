package com{
	public class Config{
		
		public static var basePath:String ;
		public static var sendErniePath:String ;//跳转path
		
		public static var bgImg:String;//奖品背景
		public var xml:XML ;
		public static var imgArray:Array ;
		
		private static var guize:int = 30 ;
		public function Config(configXml:XML,basePathStr:String){
			xml = configXml ;
			basePath = basePathStr ;
			sendErniePath = xml.sendErnie ;
			imgArray = new Array();
			bgImg = xml.winningImg.bgImg ;
			
			for each(var x:XML in xml.winningImg.winning){
				imgArray.push(x.toString());
			}
		}
		
		/**
		 * 获得中奖图片
		 * */
		public static function getWinningImg(angle:Number):String{
			var index:int = Math.round(angle%360/guize);
			trace(index+"中奖图片："+imgArray[index])
			return basePath+imgArray[index];
		}
	}
}