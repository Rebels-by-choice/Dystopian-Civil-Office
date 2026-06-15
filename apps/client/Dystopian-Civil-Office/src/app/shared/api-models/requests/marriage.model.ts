export interface CreateMarriageModel {
  registryNumber: string;
  registryDate: Date;
  spouse1Pesel: string;
  spouse2Pesel: string;
  marriageDate: Date;
  marriagePlace: string;
  documentName: string;
}

export interface UpdateMarriageModel {
  registryNumber?: string;
  registryDate?: Date;
  spouse1Pesel?: string;
  spouse2Pesel?: string;
  marriageDate?: Date;
  marriagePlace?: string;
  documentName?: string;
}

export interface DeleteMarriageModel {
  marriageRecordId: number;
}
