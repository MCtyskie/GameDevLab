using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Animations.Rigging;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using RigTestData = RuntimeRiggingTestFixture.RigTestData;


public class DampedTransformTests
{
    const float k_Epsilon = 1e-4f;

    public struct ConstraintTestData
    {
        public RigTestData rigData;
        public DampedTransform constraint;
    }

    public static ConstraintTestData SetupConstraintRig()
    {
        var data = new ConstraintTestData();

        data.rigData = RuntimeRiggingTestFixture.SetupRigHierarchy();

        var dampedTransformGO = new GameObject("dampedTransform");
        var dampedTransform = dampedTransformGO.AddComponent<DampedTransform>();
        dampedTransform.Reset();

        dampedTransformGO.transform.parent = data.rigData.rigGO.transform;

        dampedTransform.data.constrainedObject = data.rigData.hipsGO.transform.Find("Chest/LeftArm/LeftForeArm/LeftHand");

        var dampedSourceGO = new GameObject ("source");
        dampedSourceGO.transform.parent = dampedTransformGO.transform;

        dampedTransform.data.sourceObject = dampedSourceGO.transform;

        data.rigData.rootGO.GetComponent<RigBuilder>().Build();

        data.constraint = dampedTransform;

        return data;
    }

    [UnityTest]
    public IEnumerator DampedTransform_FollowsSource()
    {
        var data = SetupConstraintRig();
        var constraint = data.constraint;

        var constrainedTransform = constraint.data.constrainedObject;
        var sourceTransform = constraint.data.sourceObject;

        Vector3 constrainedPos1 = constrainedTransform.position;

        Vector3 offset = new Vector3(0f, 0.5f, 0f);
        sourceTransform.localPosition += offset;

        Vector3 constrainedPos2 = constrainedPos1 + offset;

        const int kMaxIter = 15;

        List<Vector3> positions = new List<Vector3>(kMaxIter);
        for (int i = 0; i < kMaxIter; ++i)
        {
            yield return null;
            positions.Add(constrainedTransform.position);
        }

        float[] distances = positions.Select((pos) => (pos - constrainedPos1).magnitude).ToArray();

        var floatComparer = new RuntimeRiggingTestFixture.FloatEqualityComparer(k_Epsilon);

        for (int i = 0; i < kMaxIter - 1; ++i)
        {
            Vector3 dir = positions[i + 1] - positions[i];

            Assert.That(Vector3.Angle(dir, offset), Is.EqualTo(0f).Using(floatComparer), String.Format("Offset direction mismatch at index {0}", i));
            Assert.That(distances[i + 1], Is.GreaterThanOrEqualTo(distances[i]).Using(floatComparer));
            Assert.That(distances[i], Is.LessThanOrEqualTo(0.5f).Using(floatComparer));
        }
    }

    [UnityTest]
    public IEnumerator DampedTransform_ApplyWeight()
    {
        var data = SetupConstraintRig();
        var constraint = data.constraint;
        var rigBuilder = data.rigData.rootGO.GetComponent<RigBuilder>();
        var playableGraph = rigBuilder.graph;
        playableGraph.Stop();

        // no position damping, full rotation damp
        constraint.data.dampPosition = 0f;
        constraint.data.dampRotation = 1f;

        data.constraint.weight = 0f;

        yield return null;

        var constrainedTransform = constraint.data.constrainedObject;
        var sourceTransform = constraint.data.sourceObject;

        Vector3 constrainedPos1 = constrainedTransform.position;

        Vector3 offset = new Vector3(0f, 0.5f, 0f);

        sourceTransform.localPosition += offset;
        Vector3 constrainedPos2 = constrainedPos1 + offset;

        var positionComparer = new RuntimeRiggingTestFixture.Vector3EqualityComparer(k_Epsilon);

        for (int i = 0; i <= 5; ++i)
        {
            float w = i / 5.0f;

            data.constraint.weight = w;

            // DampedTransform has a damp factor of 40.  This means it'll take 1/40 second for source to reach its target.
            // Evaluate with a large enough deltaTime to make sure source reaches its target.
            playableGraph.Evaluate(1f);
            yield return null;

            Vector3 weightedConstrainedPos = Vector3.Lerp(constrainedPos1, constrainedPos2, w);
            Vector3 constrainedPos = constrainedTransform.position;

            Assert.That(weightedConstrainedPos, Is.EqualTo(constrainedPos).Using(positionComparer), String.Format("Expected constrainedPos to be {0} for a weight of {1}, but was {2}", weightedConstrainedPos, w, constrainedPos));
        }
    }

}
