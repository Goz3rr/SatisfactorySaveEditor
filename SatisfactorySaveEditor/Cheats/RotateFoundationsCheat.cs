using SatisfactorySaveEditor.Model;
using SatisfactorySaveParser;
using SatisfactorySaveParser.Structures;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace SatisfactorySaveEditor.Cheats
{
    class RotateFoundationsCheat : ICheat
    {
        public string Name => "Rotate foundations 90, 180, 270 degrees to 0 degrees";

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            return RotateFoundations(rootItem);
        }

        private enum EnumPositionCorrectionSetting
        {
            Nothing,
            Round
        }

        private bool RotateFoundations(SaveObjectModel rootItem)
        {
            List<SaveObjectModel> foundationsList = rootItem.FindChild("Foundation", true).DescendantSelfViewModel;
            if (foundationsList == null)
            {
                MessageBox.Show("This save does not appear to have any foundations.", "Cannot find any Foundations", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            float rotationZ = 0;
            float rotationW = 1;
            double angle = 0;
            double range = 0.001;
            int counter = 0;

            EnumPositionCorrectionSetting positionCorrectionSetting;
            DialogResult dr = (DialogResult)MessageBox.Show("Correct the foundation position (X, Y, Z) to integer?", "Rotate foundations", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (dr == DialogResult.Yes) positionCorrectionSetting = EnumPositionCorrectionSetting.Round;
            else if (dr == DialogResult.No) positionCorrectionSetting = EnumPositionCorrectionSetting.Nothing;
            else
            {
                MessageBox.Show("The process has been canceled.");
                return false;
            }

            foundationsList.ForEach(target =>
            {
                Vector4 targetRotation = ((SaveEntityModel)target).Rotation;
                double targetAngle = QuaternionToEuler(targetRotation);
                // Console.WriteLine(targetRotation+"_" +targetAngle);
                if ((targetAngle > angle - range && targetAngle < angle + range) ||
                (targetAngle > angle + 90 - range && targetAngle < angle + 90 + range) ||
                (targetAngle > angle - 90 - range && targetAngle < angle - 90 + range) ||
                (targetAngle > angle + 180 - range) ||
                (targetAngle < angle - 180 + range))
                {
                    // Console.WriteLine("rotate");
                    bool isRotate = false;
                    if (targetAngle != 0)
                    {
                        targetRotation.Z = (float)rotationZ;
                        targetRotation.W = (float)rotationW;
                        ((SaveEntityModel)target).Rotation = targetRotation;
                        isRotate = true;
                    }

                    bool isCorrect = false;
                    if (positionCorrectionSetting == EnumPositionCorrectionSetting.Round)
                    {
                        Vector3 targetPosition = ((SaveEntityModel)target).Position;
                        if (targetPosition.X != (int)targetPosition.X &&
                        targetPosition.Y != (int)targetPosition.Y &&
                        targetPosition.Z != (int)targetPosition.Z)
                        {
                            targetPosition.X = (float)Math.Round(targetPosition.X);
                            targetPosition.Y = (float)Math.Round(targetPosition.Y);
                            targetPosition.Z = (float)Math.Round(targetPosition.Z);
                            ((SaveEntityModel)target).Position = targetPosition;
                            isCorrect = true;
                        }
                    }

                    if (isRotate || isCorrect) counter++;
                }
            });

            if (counter == 0)
            {
                MessageBox.Show("No changed foundation.");
                return false;
            }

            MessageBox.Show($"Successfully aligned {counter} foundations to world grid.");
            return true;
        }

        public double QuaternionToEuler(Vector4 rotation)
        {
            return Math.Atan2(rotation.Z, rotation.W) * 180 / Math.PI * 2;
        }

        public Vector4 EulerToQuaternion(double e)
        {
            var rotation = new Vector4
            {
                X = 0,
                Y = 0,
                Z = (float)Math.Round(Math.Sin(e * Math.PI / 180 / 2), 7),
                W = (float)Math.Round(Math.Cos(e * Math.PI / 180 / 2), 7)
            };
            return rotation;
        }
    }
}
