﻿using System;

namespace LinearAlgebra
{
    public enum EulerOrder
    {
        XYZ,
        XZY,
        YXZ,
        YZX,
        ZXY,
        ZYX
    }
    // do NOT ask me about this, i don't give a fuck how this works. (times tried to understand: 2)
    public struct Quaternion
    {
        public double w, x, y, z;

        public static Quaternion Identity => new Quaternion(1);
        public static Quaternion Zero => new Quaternion();

        public Quaternion(double w = 0, double x = 0, double y = 0, double z = 0)
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Dot product
        /// </summary>
        public double dot(in Quaternion q)
        {
            return w * q.w + x * q.x + y * q.y + z * q.z;
        }

        /// <summary>
        /// Norm of quaternion
        /// </summary>
        public double norm()
        {
            return dot(this);
        }

        /// <summary>
        /// Length of quaternion. Same as magnitude
        /// </summary>
        public double length()
        {
            return Math.Sqrt(norm());
        }

        /// <summary>
        /// Magnitude of quaternion. Same as length
        /// </summary>
        public double magnitude()
        {
            return length();
        }

        /// <summary>
        /// Checks if quaternion small enough to be considered a zero quaternion
        /// </summary>
        public bool isZero()
        {
            return norm() < Constants.SqrEpsilon;
        }

        /// <summary>
        /// Normalizes this quaternion
        /// </summary>
        public void normalize()
        {
            double l = length();
            w /= l;
            x /= l;
            y /= l;
            z /= l;
        }

        /// <summary>
        /// Returns normalized copy of this quaternion
        /// </summary>
        public Quaternion normalized()
        {
            return this / length();
        }

        /// <summary>
        /// Conjugates this quaternion
        /// </summary>
        public void conjugate()
        {
            x = -x;
            y = -y;
            z = -z;
        }

        /// <summary>
        /// Returns conjugated copy of this quaternion
        /// </summary>
        public Quaternion conjugated()
        {
            return new Quaternion(w, -x, -y, -z);
        }

        /// <summary>
        /// Cross product
        /// </summary>
        public Quaternion cross(in Quaternion q)
        {
            return new Quaternion(0.0, y * q.z - z * q.y, z * q.x - x * q.z, x * q.y - y * q.x);
        }

        /// <summary>
        /// Checks if quaternions are parallel enough to be considered collinear
        /// </summary>
        /// <returns>True if vectors are collinear, false otherwise</returns>
        public bool isCollinearTo(in Quaternion q)
        {
            return cross(q).isZero();
        }

        /// <summary>
        /// Returns inverse copy of this quaternion
        /// </summary>
        public Quaternion inverse()
        {
            return conjugated() / norm();
        }

        /// <summary>
        /// Inverts this quaternion
        /// </summary>
        public void invert()
        {
            double n = norm();
            conjugate();
            w /= n;
            x /= n;
            y /= n;
            z /= n;
        }

        /// <summary>
        /// Creates quaternion that represents rotation around axis for angle
        /// </summary>
        public static Quaternion FromAxisAngle(in Vector3 axis, double angle)
        {
            if (Math.Abs(axis.squaredLength() - 1.0) > Constants.SqrEpsilon)
                throw new ArgumentException("Axis is not normalized.");
            double sinhalf = Math.Sin(angle / 2.0);
            return new Quaternion(Math.Cos(angle / 2.0), axis.x * sinhalf, axis.y * sinhalf, axis.z * sinhalf);
        }

        public static Quaternion FromMatrix(in Matrix3x3 rot)
        {
            double trace = rot.v00 + rot.v11 + rot.v22;

            if (trace > 0)
            {
                double S = 0.5 / Math.Sqrt(trace + 1.0); // S=1/4qw
                return new Quaternion(0.25 / S, (rot.v21 - rot.v12) * S, (rot.v02 - rot.v20) * S, (rot.v10 - rot.v01) * S);
            }
            else if (rot.v00 > rot.v11 && rot.v00 > rot.v22)
            {
                double S = 0.5 / Math.Sqrt(1.0 + rot.v00 - rot.v11 - rot.v22); // S=1/4qx
                return new Quaternion((rot.v21 - rot.v12) * S, 0.25 / S, (rot.v01 + rot.v10) * S, (rot.v02 + rot.v20) * S);
            }
            else if (rot.v11 > rot.v22)
            {
                double S = 0.5 / Math.Sqrt(1.0 - rot.v00 + rot.v11 - rot.v22); // S=1/4qy
                return new Quaternion((rot.v02 - rot.v20) * S, (rot.v01 + rot.v10) * S, 0.25 / S, (rot.v12 + rot.v21) * S);
            }
            else
            {
                double S = 0.5 / Math.Sqrt(1.0 - rot.v00 - rot.v11 + rot.v22); // S=1/4qz
                return new Quaternion((rot.v10 - rot.v01) * S, (rot.v02 + rot.v20) * S, (rot.v12 + rot.v21) * S, 0.25 / S);
            }
        }

        public static Quaternion FromMatrix(in Matrix4x4 rot)
        {
            double trace = rot.v00 + rot.v11 + rot.v22;

            if (trace > 0)
            {
                double S = 0.5 / Math.Sqrt(trace + 1.0); // S=1/4qw
                return new Quaternion(0.25 / S, (rot.v21 - rot.v12) * S, (rot.v02 - rot.v20) * S, (rot.v10 - rot.v01) * S);
            }
            else if (rot.v00 > rot.v11 && rot.v00 > rot.v22)
            {
                double S = 0.5 / Math.Sqrt(1.0 + rot.v00 - rot.v11 - rot.v22); // S=1/4qx
                return new Quaternion((rot.v21 - rot.v12) * S, 0.25 / S, (rot.v01 + rot.v10) * S, (rot.v02 + rot.v20) * S);
            }
            else if (rot.v11 > rot.v22)
            {
                double S = 0.5 / Math.Sqrt(1.0 - rot.v00 + rot.v11 - rot.v22); // S=1/4qy
                return new Quaternion((rot.v02 - rot.v20) * S, (rot.v01 + rot.v10) * S, 0.25 / S, (rot.v12 + rot.v21) * S);
            }
            else
            {
                double S = 0.5 / Math.Sqrt(1.0 - rot.v00 - rot.v11 + rot.v22); // S=1/4qz
                return new Quaternion((rot.v10 - rot.v01) * S, (rot.v02 + rot.v20) * S, (rot.v12 + rot.v21) * S, 0.25 / S);
            }
        }

        /// <summary>
        /// Creates quaternion from euler angles in specified order
        /// </summary>
        public static Quaternion FromEuler(in Vector3 euler, EulerOrder order = EulerOrder.ZXY)
        {
            double sinhalfx = Math.Sin(euler.x / 2.0);
            double sinhalfy = Math.Sin(euler.y / 2.0);
            double sinhalfz = Math.Sin(euler.z / 2.0);
            double coshalfx = Math.Cos(euler.x / 2.0);
            double coshalfy = Math.Cos(euler.y / 2.0);
            double coshalfz = Math.Cos(euler.z / 2.0);
            switch (order)
            {
                case EulerOrder.XYZ:
                    return new Quaternion(coshalfz, 0.0, 0.0, sinhalfz) *
                           new Quaternion(coshalfy, 0.0, sinhalfy, 0.0) *
                           new Quaternion(coshalfx, sinhalfx, 0.0, 0.0);
                case EulerOrder.XZY:
                    return new Quaternion(coshalfy, 0.0, sinhalfy, 0.0) *
                           new Quaternion(coshalfz, 0.0, 0.0, sinhalfz) *
                           new Quaternion(coshalfx, sinhalfx, 0.0, 0.0);
                case EulerOrder.YXZ:
                    return new Quaternion(coshalfz, 0.0, 0.0, sinhalfz) *
                           new Quaternion(coshalfx, sinhalfx, 0.0, 0.0) *
                           new Quaternion(coshalfy, 0.0, sinhalfy, 0.0);
                case EulerOrder.YZX:
                    return new Quaternion(coshalfx, sinhalfx, 0.0, 0.0) *
                           new Quaternion(coshalfz, 0.0, 0.0, sinhalfz) *
                           new Quaternion(coshalfy, 0.0, sinhalfy, 0.0);
                case EulerOrder.ZXY:
                    return new Quaternion(coshalfy, 0.0, sinhalfy, 0.0) *
                           new Quaternion(coshalfx, sinhalfx, 0.0, 0.0) *
                           new Quaternion(coshalfz, 0.0, 0.0, sinhalfz);
                case EulerOrder.ZYX:
                    return new Quaternion(coshalfx, sinhalfx, 0.0, 0.0) *
                           new Quaternion(coshalfy, 0.0, sinhalfy, 0.0) *
                           new Quaternion(coshalfz, 0.0, 0.0, sinhalfz);
            }
            throw new NotImplementedException();
        }

        public static Quaternion operator /(in Quaternion lhs, double rhs)
        {
            return new Quaternion(lhs.w / rhs, lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
        }

        public static Quaternion operator *(in Quaternion lhs, double rhs)
        {
            return new Quaternion(lhs.w * rhs, lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        }

        public static Quaternion operator *(double lhs, in Quaternion rhs)
        {
            return new Quaternion(lhs * rhs.w, lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
        }

        public static Quaternion operator *(in Quaternion lhs, in Quaternion rhs)
        {
            return new Quaternion(lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z,
                                  lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                                  lhs.w * rhs.y - lhs.x * rhs.z + lhs.y * rhs.w + lhs.z * rhs.x,
                                  lhs.w * rhs.z + lhs.x * rhs.y - lhs.y * rhs.x + lhs.z * rhs.w);
        }

        public static Vector3 operator *(in Quaternion lhs, in Vector3 rhs)
        {
            Quaternion q = lhs * new Quaternion(0.0, rhs.x, rhs.y, rhs.z) * lhs.inverse();
            return new Vector3(q.x, q.y, q.z);
        }

        public static Quaternion operator +(in Quaternion lhs, in Quaternion rhs)
        {
            return new Quaternion(lhs.w + rhs.w, lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        public static Quaternion operator -(in Quaternion lhs, in Quaternion rhs)
        {
            return new Quaternion(lhs.w - rhs.w, lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public bool Equals(in Quaternion q)
        {
            if (Math.Abs(w - q.w) > Constants.Epsilon ||
                Math.Abs(x - q.x) > Constants.Epsilon ||
                Math.Abs(y - q.y) > Constants.Epsilon ||
                Math.Abs(z - q.z) > Constants.Epsilon)
                return false;
            return true;
        }

        public override string ToString()
        {
            return "(" + w.ToString() + ", " + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";
        }

        public string ToString(string format)
        {
            return "(" + w.ToString(format) + ", " + x.ToString(format) + ", " + y.ToString(format) + ", " + z.ToString(format) + ")";
        }
    }
}