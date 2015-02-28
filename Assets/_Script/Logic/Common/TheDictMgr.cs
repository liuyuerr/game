using UnityEngine;
using System.Collections;
using Dylib;

public class TheDictMgr : DictMgr {

	#region dicts
	public IDict roleDict = null;
	public IDict roleDict2 = null;
	public IDict skillDict = null;
	public IDict mapDict = null;
	public IDict actionInfoDict = null;
	public IDict animInfoDict = null;
	#endregion
	
	public override void InitDicts () {
//		roleDict = initDictWithCsv ("Assets/csv/role", typeof(BaseDict), typeof(RoleInfo)); 	   //18ms
//		skillDict = initDictWithCsv ("Assets/csv/skill", typeof(SkillDict), typeof(SkillInfo));
//		mapDict = initDictWithCsv("Assets/csv/map", typeof(BaseDict), typeof(Mapinfo));
//		actionInfoDict = initDictWithCsv("Assets/csv/action", typeof(BaseDict), typeof(ActionInfo));
//		animInfoDict = initDictWithCsv("Assets/csv/animation", typeof(BaseDict), typeof(AnimationInfo));

		//todo: put _vInit flag to the handler when dict load complete;
	}

}
