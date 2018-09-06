import { FixedParameter } from "./parameters";

export interface Experiment {
  id?: number;
  name?: string;
  creator?: string;
  description?: string;
  payloadFilename?: string;
  payloadHash?: string;
  sharedFixedParameter?: FixedParameter[];
}
