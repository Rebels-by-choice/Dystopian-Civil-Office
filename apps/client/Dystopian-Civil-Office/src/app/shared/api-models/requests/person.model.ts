export interface CreatePersonModel {
  pesel: string;
  firstName: string;
  middleName: string;
  lastName: string;
  gender: string;
  birthDate: Date;
  birthPlace: string;
  addressRegistryNumber: string;
  documentName: string;
}

export interface UpdatePersonModel {
  pesel?: string;
  firstName?: string;
  middleName?: string;
  lastName?: string;
  gender?: string;
  birthDate?: Date;
  birthPlace?: string;
  addressRegistryNumber?: string;
  documentName?: string;
}

export interface DeletePersonModel {
  personId: number;
}
