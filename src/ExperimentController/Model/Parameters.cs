using System;
using System.ComponentModel.DataAnnotations.Schema;
using JsonSubTypes;
using Newtonsoft.Json;

namespace ExperimentController.Model
{
    [JsonConverter (typeof (JsonSubtypes))]
    [JsonSubtypes.KnownSubTypeWithProperty (typeof (DoubleParameter), "DoubleValue")]
    [JsonSubtypes.KnownSubTypeWithProperty (typeof (IntParameter), "IntValue")]
    [JsonSubtypes.KnownSubTypeWithProperty (typeof (StringParameter), "StringValue")]
    [JsonSubtypes.KnownSubTypeWithProperty (typeof (BoolParameter), "BoolValue")]
    public abstract class FixedParameter
    {
        public string Name;

        [NotMapped]
        [JsonIgnore]
        public abstract object Value { get; set; }

        public string ParameterValueType { get; set; }

        public abstract FixedParameter Clone ();
    }

    public class DoubleParameter : FixedParameter
    {
        [JsonIgnore]
        [NotMapped]
        public override object Value
        {
            get { return DoubleValue; }
            set
            {
                if (value.GetType () == typeof (double))
                {
                    DoubleValue = (double) value;
                }
                else
                {
                    throw new ArgumentException ("The given value has to be of type double");
                }
            }
        }

        public double DoubleValue { get; set; }

        public override FixedParameter Clone ()
        {
            return new DoubleParameter () { Name = this.Name, DoubleValue = this.DoubleValue };
        }
    }

    public class IntParameter : FixedParameter
    {
        [JsonIgnore]
        [NotMapped]
        public override object Value
        {
            get { return IntValue; }
            set
            {
                if (value.GetType () == typeof (int))
                {
                    IntValue = (int) value;
                }
                else
                {
                    throw new ArgumentException ("The given value has to be of type int");
                }
            }
        }

        public int IntValue { get; set; }

        public override FixedParameter Clone ()
        {
            return new IntParameter () { Name = this.Name, IntValue = this.IntValue };
        }
    }

    public class StringParameter : FixedParameter
    {
        [JsonIgnore]
        [NotMapped]
        public override object Value
        {
            get { return StringValue; }
            set
            {
                if (value.GetType () == typeof (string))
                {
                    StringValue = (string) value;
                }
                else
                {
                    throw new ArgumentException ("The given value has to be of type string");
                }
            }
        }

        public string StringValue { get; set; }

        public override FixedParameter Clone ()
        {
            return new StringParameter () { Name = this.Name, StringValue = this.StringValue };
        }
    }

    public class BoolParameter : FixedParameter
    {
        [JsonIgnore]
        [NotMapped]
        public override object Value
        {
            get { return BoolValue; }
            set
            {
                if (value.GetType () == typeof (bool))
                {
                    BoolValue = (bool) value;
                }
                else
                {
                    throw new ArgumentException ("The given value has to be of type bool");
                }
            }
        }

        public bool BoolValue { get; set; }

        public override FixedParameter Clone ()
        {
            return new BoolParameter () { Name = this.Name, BoolValue = this.BoolValue };
        }
    }

}