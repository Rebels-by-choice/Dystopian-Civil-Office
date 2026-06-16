export interface CreateBirthRecordModel {
  RegistryNumber: string;
  PersonPesel: string;
  DeathDate: Date;
  DeathPlace: string;
  RegistryDate: Date;
  CauseOfDeath: string;
  DocumentName: string;
}

export interface UpdateBirthRecordModel {
  RegistryNumber?: string;
  PersonPesel?: string;
  DeathDate?: Date;
  DeathPlace?: string;
  RegistryDate?: Date;
  CauseOfDeath?: string;
  DocumentName?: string;
}

export interface DeleteBirthRecordModel {
  BirthRecordId: number;
}
