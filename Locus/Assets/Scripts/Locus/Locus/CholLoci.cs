using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrsUtils.BehaviorTree;
using LocusNodes;

namespace Locus
{
	//	Fire Ruling
	public class CholLoci : BasicLocus 
	{
		// Use this for initialization
		override protected void Start () 
		{
			m_Tree = 
			new Tree<BasicLocus>
			(
				new Selector<BasicLocus>
				(
					new Wander()
				)
			);
		}
	}
}
