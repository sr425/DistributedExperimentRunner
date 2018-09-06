import { Dataset } from './dataset';
import { FixedParameter, OptimicationParameter } from './parameters';

export interface ExperimentPart {
  id?: number;
  name?: string;
  experimentId?: number;
  inputDatasets?: Dataset[];
  fixedParameters?: FixedParameter[];
  dynamicParameters?: OptimicationParameter[];

  aggregatedValues?: { [key: string]: number };

  running?: boolean;
  failed?: boolean;
  finished?: boolean;
}
