﻿using System.Collections;
using System.Collections.Generic;
using Mapbox.Utils;
using UnityEngine;

public class TaskLab
{
    private static TaskLab mLatlngLab;
    private Vector2d CurrentLatlng;
    private List<Task> TaskList = new List<Task>();
    private TaskLab()
    {
        TaskList.Add(new Task() { TaskModelName = "Capsule", TaskModelRotation = Quaternion.Euler(Vector3.zero), TaskLocation = new Vector2d(39.1517, 117.460854), TaskDesc = "《白头吟》 两汉卓文君" });
        TaskList.Add(new Task() { TaskModelName = "Capsule", TaskModelRotation = Quaternion.Euler(Vector3.zero), TaskLocation = new Vector2d(39.1502724, 117.461151), TaskDesc = "皑如山上雪,皎若云间月。 闻君有两意,故来相决绝。" });
        TaskList.Add(new Task() { TaskModelName = "Capsule", TaskModelRotation = Quaternion.Euler(Vector3.zero), TaskLocation = new Vector2d(39.1505737, 117.460945), TaskDesc = "今日斗酒会，明旦沟水头。 躞蹀御沟上，沟水东西流。" });

        TaskList.Add(new Task() { TaskModelName = "Capsule", TaskModelRotation = Quaternion.Euler(Vector3.zero), TaskLocation = new Vector2d(60.19175, 24.9685821f), TaskDesc = "凄凄复凄凄，嫁娶不须啼。 愿得一心人，白头不相离。" });
        TaskList.Add(new Task() { TaskModelName = "Capsule", TaskModelRotation = Quaternion.Euler(Vector3.zero), TaskLocation = new Vector2d(60.19287, 24.9675821f), TaskDesc = "竹竿何袅袅，鱼尾何簁簁！ 男儿重意气，何用钱刀为！" });
    }

    public static TaskLab get()
    {
        if (mLatlngLab == null)
            mLatlngLab = new TaskLab();
        return mLatlngLab;
    }

    public void SetCurrentLatlng(Vector2d currentLatlng)
    {
        CurrentLatlng = currentLatlng;
    }

    public List<Task> GetTaskListIn(int meter)
    {
        List<Task> tasks = TaskList.FindAll((task) =>
        {
            return ARUtils.GetDistance(task.TaskLocation, CurrentLatlng) <= meter;
        });
        return tasks;
    }

    public List<Task> GetTaskList()
    {
        return TaskList;
    }

}
