# 编程辅助库

## SingletonEx 
轻松创建单例 只需要继承 Singleton<T> 即可，然后把构造函数（私有，无参）作为初始化函数，只在第一次调用时初始化

## ColorUtil
帮助转换Unity Color 为 16进制表示
- Color HexToColor(string _Hex)
- string ToHex(this Color _Color)

返回 形如"000000FF"形式的字符串，最后只需要在前面加上'#',就可以作为Unity的富文本使用

 如 `_Text.text = (string.Format("<color=\"#{0}\">{1}</color>", _Color.ToHex(), _Content));`

## TextUtil
封装了上面的颜色，可以轻松使用富文本的颜色设置
`RichText(this Text _Text, string _Content, Color _Color)`
`RichText(this Text _Text, string _Content, float _R, float _G, float _B, float _A = 1f)`

## LogUtil
colorful Log

## DataUtil
对一些常用数据类型进行扩展，来简化使用，
如
- Dictionary 的GetKeys,GetValues 直接返回结果数组
- List 添加Pop 和 Push 接口，可以向Stack一样的逻辑
- ListPool，队列池，复用List<T>，减少内存开辟消耗
- StringEx
	- 检测是否以另一个字符串结尾
	- 检测是否以某个字符串开始
	- 移除末尾n个字符

## 加密与混淆

## FileUtil
文件创建，读取，写入操作