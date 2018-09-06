export interface FixedParameter {
  name?: string;
  parameterValueType?: string;
  doubleValue?: number;
  intValue?: number;
  stringValue?: string;
  boolValue?: boolean;
}

export interface OptimicationParameter {
  id?: number;
  name?: string;
  parameterValueType?: string;
  optimizerParams?: { [key: string]: any };
}

export class DoubleFixedParameter implements FixedParameter {
  name?: string;
  doubleValue?: number;
}

export class IntFixedParameter implements FixedParameter {
  name?: string;
  intValue?: number;
}

export class StringFixedParameter implements FixedParameter {
  name?: string;
  stringValue?: string;
}

export class BoolFixedParameter implements FixedParameter {
  name?: string;
  boolValue?: boolean;
}
