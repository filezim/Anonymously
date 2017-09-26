package br.com.diego.agenda.dao;

import java.util.List;

import org.junit.Ignore;
import org.junit.Test;

import br.com.diego.agenda.entity.Person;

public class PersonDAOTest {
	@Test
	@Ignore
	public void save() {
		Person person = new Person();
		person.setName("Gustavo Lima");
		person.setAddress("Rua Primeiro de Maio");
		person.setCellPhone("0000-0000");
		person.setCity("Teresina");
		person.setComplement("Proximo ao senai");
		person.setCpf("000.000.000-52");
		person.setDistrict("PI");
		person.setEmail("diego@godking.com");
		person.setIdenty("25.631.256-98");
		person.setPostalCode("64002-510");
		person.setLandline("3215-7592");

		PersonDAO personDAO = new PersonDAO();
		personDAO.save(person);
	}

	@Test
	@Ignore
	public void list() {
		PersonDAO personDAO = new PersonDAO();
		List<Person> result = personDAO.list();

		System.out.println("((( REGISTROS )))");
		for (Person person : result) {
			System.out.println(person.getName() + " <=>" + person.getCity());
		}
	}

	@Test
	@Ignore
	public void search() {
		Long code = 1L;

		PersonDAO personDAO = new PersonDAO();
		Person person = personDAO.search(code);

		if (person == null) {
			System.out.println("Não foi encontrado porra nenhuma! Animal!");
		} else {
			System.out.println(person.getName() + " <=>" + person.getCity());
		}

	}

	@Test
	@Ignore
	public void delete() {
		Long code = 1L;

		PersonDAO personDAO = new PersonDAO();
		Person person = personDAO.search(code);

		if (person == null) {
			System.out.println("Não foi encontrado porra nenhuma! Animal!");
		} else {
			personDAO.delete(person);
			System.out.println("((( REGISTRO EXLCUIDO )))");
			System.out.println(person.getName() + " <=>" + person.getCity());
		}

	}

	@Test
	@Ignore
	public void update() {
		Long code = 2L;

		PersonDAO personDAO = new PersonDAO();
		Person person = personDAO.search(code);
		
		if (person == null) {
			System.out.println("Nada foi encontrado... seu ignorante!");
		} else {
			person.setName("Mike Bernado");
			personDAO.update(person);
			
			System.out.println("REGISTRO ALTERADO!!");
			System.out.println(person.getName() + " <=>" + person.getCity());
		}

	}
//	@Test
//	@Ignore
//	public void merge() {
//		Person person = new Person();
//		person.setName("Diego Parente");
//		person.setAddress("Rua Primeiro de Maio");
//		person.setCellPhone("0000-0000");
//		person.setCity("Teresina");
//		person.setComplement("Proximo ao senai");
//		person.setCpf("000.000.000-52");
//		person.setDistrict("PI");
//		person.setEmail("diego@godking.com");
//		person.setIdenty("25.631.256-98");
//		person.setPostalCode("64002-510");
//		person.setLandline("3215-7592");
//
//		PersonDAO personDAO = new PersonDAO();
//		personDAO.merge(person);
//	}
	
	
}
