export interface CreateBirthRecordModel {
  registryNumber: string;
  personPesel: string;
  motherPesel: string;
  fatherPesel: string;
  registryDate: Date;
  documentName: string;
}

export interface UpdateBirthRecordModel {
  registryNumber?: string;
  personPesel?: string;
  motherPesel?: string;
  fatherPesel?: number;
  registryDate?: Date;
  documentName?: string;
}

export interface DeleteBirthRecordModel {
  birthRecordId: number;
}
