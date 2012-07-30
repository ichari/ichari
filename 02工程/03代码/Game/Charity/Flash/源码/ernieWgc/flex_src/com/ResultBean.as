package com{
	/**点击摇奖后flex调用  注意直接把角度传过来
	 * type : checkLogin 这个值是死的 提供 flex端扩展性
	 * status ：1：成功	0：失败
	 * url	: 跳转的路径		空字符就不跳转
	 * target ：页面打开方式
	 * massage ：需要弹出显示个用户看的信息  空就不显示
	 * angle ：角度
	 * 注意为空 null 或不写 就不会起作用
	 * 数据为json格式的
	 */
	public class ResultBean{
		public var userId:String ;
		public var type:String ;//这里是我区分那个方法 checkLogin
		public var status:int = 1 ;//1：成功	0：失败
		public var url:String ;//跳转的路径		空字符就不跳转
		public var target:String ;//页面打开方式
		public var massage:String ;//需要弹出显示个用户看的信息  空就不显示
		public var angle:String ;//角度
		
		public function ResultBean(){
			
		}
	}
}